using Core.Auth.domain;
using HNS.Model;
using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace HNS
{
    public class GameUpdatesListener : MonoBehaviour
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        [Inject] private IAuthRepository authRepository;

        [SerializeField] private Transform playersRoot;
        [SerializeField] private GameObject finishedScreen;
        [SerializeField] private SeekerController seekerControllerPrefab;
        [SerializeField] private HiderControllerPlayer hiderControllerPrefab;

        private readonly CompositeDisposable enabledLifecycle = new();

        private readonly ReactiveDictionary<long, SeekerSnapshotItem> seekers = new();
        private readonly ReactiveDictionary<long, HiderSnapshotItem> hiders = new();

        private BehaviorSubject<GameStates> gameState = new(GameStates.Pending);

        private void OnEnable()
        {
            finishedScreen.SetActive(false);
        
            commandsUseCase
                .Subscribe<HNSGameUpdate>(Commands.RoundUpdates)
                .Subscribe(OnGameUpdate)
                .AddTo(enabledLifecycle);

            seekers
                .ObserveAdd()
                .Subscribe(data => CreateSeeker(data.Key, data.Value))
                .AddTo(enabledLifecycle);

            hiders
                .ObserveAdd()
                .Subscribe(data => CreateHider(data.Key, data.Value))
                .AddTo(enabledLifecycle);
        
            commandsUseCase
                .Subscribe<long>(Commands.GameState)
                .DistinctUntilChanged()
                .Select(code => (GameStates)code)
                .Subscribe(gameState.OnNext)
                .AddTo(enabledLifecycle);
        }

        private void CreateHider(long id, HiderSnapshotItem initialSnapshot)
        {
            var controller = Instantiate(hiderControllerPrefab, playersRoot);
            controller.transform.ApplySnapshot(initialSnapshot.transform);
            controller.isPlayer = authRepository.LoginUserId == id.ToString();
            controller.isMovable = () => gameState.Value is GameStates.Hiding or GameStates.Searching;
            hiders
                .ObserveRemove()
                .Where(data => data.Key == id)
                .Subscribe(data => Destroy(controller))
                .AddTo(enabledLifecycle);
            hiders
                .ObserveReplace()
                .Where(data => data.Key == id)
                .Subscribe(data => controller.SubmitSnapshot(data.NewValue))
                .AddTo(enabledLifecycle);
        }

        private void CreateSeeker(long id, SeekerSnapshotItem initialSnapshot)
        {
            var controller = Instantiate(seekerControllerPrefab, playersRoot);
            controller.transform.ApplySnapshot(initialSnapshot.transform);
            controller.isPlayer = authRepository.LoginUserId == id.ToString();
            controller.isMovable = () => gameState.Value is GameStates.Searching;
            seekers
                .ObserveRemove()
                .Where(data => data.Key == id)
                .Subscribe(data => Destroy(controller))
                .AddTo(enabledLifecycle);
            seekers
                .ObserveReplace()
                .Where(data => data.Key == id)
                .Subscribe(data => controller.SubmitSnapshot(data.NewValue))
                .AddTo(enabledLifecycle);
        }

        private void OnGameUpdate(HNSGameUpdate update)
        {
            var snapshot = update.snapshot;
            foreach (var (key, seekerSnapshotItem) in snapshot.Seekers)
                seekers[key] = seekerSnapshotItem;

            foreach (var (key, hiderSnapshotItem) in snapshot.Hiders)
                hiders[key] = hiderSnapshotItem;

            if (update.isFinished)
            {
                finishedScreen.SetActive(true);
            }
        }

        private void OnDisable()
        {
            seekers.Clear();
            hiders.Clear();
            while (playersRoot.childCount > 0)
            {
                DestroyImmediate(playersRoot.GetChild(0).gameObject);
            }

            enabledLifecycle.Dispose();
            gameState = new(GameStates.Pending);
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
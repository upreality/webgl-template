using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace WSExample
{
    public class SimpleLobbyScreen : MonoBehaviour
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        [SerializeField] private RectTransform loader;
        [SerializeField] private Button enterLobbyButton;
        [SerializeField] private Button exitLobbyButton;

        private const long EnterLobbyIntentCode = 0L;
        private const long ExitLobbyIntentCode = 1L;

        private void Start()
        {
            commandsUseCase
                .Subscribe<long>(Commands.LobbyState)
                .DistinctUntilChanged()
                .Subscribe(HandleLobbyState)
                .AddTo(this);

            enterLobbyButton.onClick.AddListener(JoinLobby);
            exitLobbyButton.onClick.AddListener(LeaveLobby);
        }

        private void HandleLobbyState(long stateCode)
        {
            var inLobby = stateCode == 1;
            Debug.Log("Lobby state = " + inLobby);
            loader.gameObject.SetActive(inLobby);
            enterLobbyButton.interactable = !inLobby;
            exitLobbyButton.interactable = inLobby;
        }

        private void JoinLobby() => commandsUseCase
            .Request<long, long>(Commands.LobbyAction, EnterLobbyIntentCode)
            .DistinctUntilChanged()
            .Subscribe()
            .AddTo(this);

        private void LeaveLobby() => commandsUseCase
            .Request<long, long>(Commands.LobbyAction, ExitLobbyIntentCode)
            .DistinctUntilChanged()
            .Subscribe()
            .AddTo(this);
    }
}
using HNS.domain;
using HNS.domain.Model;
using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace HNS.data
{
    public class GameUpdatesObserver : MonoBehaviour
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        [Inject] private HNSPlayerSnapshotsUseCase playerSnapshotsUseCase;
        [Inject] private HNSGameStateUseCase gameStateUseCase;

        private readonly CompositeDisposable enabledLifecycle = new();

        private void OnEnable()
        {
            commandsUseCase
                .Subscribe<HNSGameUpdate>(Commands.RoundUpdates)
                .Subscribe(OnGameUpdate)
                .AddTo(enabledLifecycle);
        
            commandsUseCase
                .Subscribe<long>(Commands.GameState)
                .DistinctUntilChanged()
                .Select(code => (GameStates)code)
                .Subscribe(gameStateUseCase.SubmitState)
                .AddTo(enabledLifecycle);
        }

        private void OnGameUpdate(HNSGameUpdate update)
        {
            var snapshot = update.snapshot;
            
            foreach (var (key, seekerSnapshotItem) in snapshot.Seekers)
                playerSnapshotsUseCase.Submit(key, seekerSnapshotItem);

            foreach (var (key, hiderSnapshotItem) in snapshot.Hiders)
                playerSnapshotsUseCase.Submit(key, hiderSnapshotItem);

            if (!update.isFinished) 
                return;
            
            ClearSubscriptions();
        }

        private void ClearSubscriptions()
        {
            playerSnapshotsUseCase.Clear();
            gameStateUseCase.Complete();
            enabledLifecycle.Dispose();
        }

        private void OnDisable() => ClearSubscriptions();
    }
}
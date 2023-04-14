using System;
using Multiplayer.PlayerState.domain;
using Multiplayer.PlayerState.domain.model;
using Multiplayer.PlayerState.domain.repositories;
using UniRx;
using Utils.ZenjectCodegen;
using Zenject;

namespace Multiplayer.PlayerState.data
{
    // [SceneSubscriptionHandler]
    public class PlayerStateSelfUpdateRepository : IPlayerStateRepository
    {
        [Inject] private PlayerStateUpdatesUseCase updatesUseCase;

        private readonly ReactiveProperty<PlayerStates> stateProperty = new(PlayerStates.None);

        // [SceneSubscription]
        public IDisposable HandleUpdates() => updatesUseCase
            .GetPlayerStateUpdatesFlow()
            .Subscribe(state => stateProperty.Value = state);
        public IObservable<PlayerStates> GetPlayerStateFlow() => stateProperty;
        public PlayerStates GetPlayerState() => stateProperty.Value;
    }
}
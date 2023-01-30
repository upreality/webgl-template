using System;
using Multiplayer.PlayerState.domain.model;
using Multiplayer.PlayerState.domain.repositories;
using UniRx;
using Zenject;

namespace Multiplayer.Spawn.domain
{
    public class ReadyAfterDeathUseCase
    {
        [Inject] private IPlayerStateRepository playerStateRepository;

        public ReactiveCommand GetReadyCommandFlow(int readyDelay) => playerStateRepository
            .GetPlayerStateFlow()
            .Select(state => GetBecomeReadyFlow(state, readyDelay))
            .Switch()
            .ToReactiveCommand();

        private static IObservable<bool> GetBecomeReadyFlow(PlayerStates state, int readyDelay) => state == PlayerStates.Dead
            ? Observable.Timer(TimeSpan.FromSeconds(readyDelay)).Select(_ => true)
            : Observable.Return(false);
    }
}
using System;
using Multiplayer.PlayerState.domain.model;
using Multiplayer.PlayerState.domain.repositories;
using UniRx;
using Zenject;

namespace Multiplayer.Spawn.domain
{
    public class SpawnPlayerAvailableUseCase
    {
        [Inject] private IPlayerStateRepository playerStateRepository;

        public IObservable<bool> GetSpawnAvailableFlow() => playerStateRepository
            .GetPlayerStateFlow()
            .Select(state => state == PlayerStates.Spawning)
            .DistinctUntilChanged();

        public bool GetSpawnAvailable() => playerStateRepository.GetPlayerState() == PlayerStates.Spawning;
    }
}
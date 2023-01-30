using System;
using Multiplayer.MatchState.domain.model;
using Multiplayer.MatchState.domain.repositories;
using Multiplayer.SpawnPoints.domain.repositories;
using UniRx;
using Zenject;

namespace Multiplayer.SpawnPoints.domain
{
    public class SpawnPointAvailableUseCase
    {
        [Inject] private IMatchStateRepository matchStateRepository;
        [Inject] private ISpawnPointCooldownRepository spawnPointCooldownRepository;
        public IObservable<bool> GetSpawnAvailableFlow(int pointId)
        {
            var cooldownFlow = spawnPointCooldownRepository.GetCooldownFlow(pointId);
            return matchStateRepository
                .GetMatchStateFlow()
                .CombineLatest(cooldownFlow, GetSpawnAvailable)
                .DistinctUntilChanged();
        }

        public bool GetSpawnAvailable(int pointId)
        {
            var state = matchStateRepository.GetMatchState();
            var cooldown = spawnPointCooldownRepository.GetCooldown(pointId);
            return GetSpawnAvailable(state, cooldown);
        }

        private static bool GetSpawnAvailable(MatchStates state, int cooldown)
        {
            return state == MatchStates.Playing && cooldown <= 0;
        }
    }
}
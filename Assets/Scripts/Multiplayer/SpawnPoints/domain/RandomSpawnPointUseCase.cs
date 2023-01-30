using System;
using System.Collections.Generic;
using System.Linq;
using Multiplayer.SpawnPoints.domain.repositories;
using UniRx;
using Zenject;
using Random = UnityEngine.Random;

namespace Multiplayer.SpawnPoints.domain
{
    public class RandomSpawnPointUseCase
    {
        [Inject] private SpawnPointAvailableUseCase spawnPointAvailableUseCase;
        [Inject] private ISpawnPointsRepository spawnPointsRepository;

        public IObservable<int> GetPointId()
        {
            var pointIds = spawnPointsRepository.GetPoints().Select(point => point.PointId).ToList();
            var availablePoints = pointIds.Where(spawnPointAvailableUseCase.GetSpawnAvailable).ToList();

            if (!availablePoints.Any())
                return GetFirstAvailableSpawnPoint(pointIds);

            var point = availablePoints[Random.Range(0, availablePoints.Count)];
            return Observable.Return(point);
        }

        private IObservable<int> GetFirstAvailableSpawnPoint(List<int> pointIds) => pointIds.Select(pointId =>
            spawnPointAvailableUseCase
                .GetSpawnAvailableFlow(pointId)
                .Where(available => available)
                .Select(_ => pointId)
        ).Merge().Take(1);
    }
}
using System.Collections.Generic;
using System.Linq;
using Multiplayer.Spawn.domain.model;
using Multiplayer.SpawnPoints.domain.repositories;

namespace Multiplayer.SpawnPoints.data
{
    public class SpawnPointsDefaultRepository : ISpawnPointsRepository
    {
        private const int DEFAULT_COOLDOWN = 60;
        private readonly Dictionary<int, SpawnPoint> points = new();

        public SpawnPoint GetSpawnPoint(int pointId) => points.TryGetValue(pointId, out var point)
            ? point
            : new SpawnPoint(pointId, DEFAULT_COOLDOWN);

        public List<SpawnPoint> GetPoints() => points.Values.ToList();

        void ISpawnPointsRepository.AddSpawnPoint(SpawnPoint point) => points[point.PointId] = point;
    }
}
using System.Collections.Generic;
using Multiplayer.Spawn.domain.model;

namespace Multiplayer.SpawnPoints.domain.repositories
{
    public interface ISpawnPointsRepository
    {
        public SpawnPoint GetSpawnPoint(int pointId);
        public List<SpawnPoint> GetPoints();
        internal void AddSpawnPoint(SpawnPoint point);
    }
}
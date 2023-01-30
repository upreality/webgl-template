using System;
using Multiplayer.Spawn.domain.model;

namespace Multiplayer.Spawn.domain.repositories
{
    public interface IPlayerSpawnEventRepository
    {
        internal void AddSpawnEvent(SpawnEvent spawnEvent);
        public IObservable<SpawnEvent> GetSpawnEventFlow();
    }
}
using System;
using Multiplayer.Spawn.domain.model;
using Multiplayer.Spawn.domain.repositories;
using UniRx;

namespace Multiplayer.Spawn.data
{
    public class PlayerSpawnEventDefaultRepository : IPlayerSpawnEventRepository
    {
        private readonly Subject<SpawnEvent> spawnPointSubject = new();
        
        void IPlayerSpawnEventRepository.AddSpawnEvent(SpawnEvent spawnEvent) => spawnPointSubject.OnNext(spawnEvent);
        IObservable<SpawnEvent> IPlayerSpawnEventRepository.GetSpawnEventFlow() => spawnPointSubject;
    }
}
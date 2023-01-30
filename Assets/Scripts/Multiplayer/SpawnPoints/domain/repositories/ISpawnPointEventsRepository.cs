using System;
using Multiplayer.SpawnPoints.domain.model;

namespace Multiplayer.SpawnPoints.domain.repositories
{
    public interface ISpawnPointEventsRepository
    {
        internal void AddEvent(int pointId, int cooldown);
        public IObservable<ActivatePointEvent> GetEventFlow();
    }
}
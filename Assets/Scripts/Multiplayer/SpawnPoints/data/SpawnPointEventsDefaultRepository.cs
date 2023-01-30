using System;
using Multiplayer.SpawnPoints.domain.model;
using Multiplayer.SpawnPoints.domain.repositories;
using UniRx;

namespace Multiplayer.SpawnPoints.data
{
    public class SpawnPointEventsDefaultRepository: ISpawnPointEventsRepository
    {
        private readonly Subject<ActivatePointEvent> eventsSubject = new();

        void ISpawnPointEventsRepository.AddEvent(int pointId, int cooldown)
        {
            eventsSubject.OnNext(new ActivatePointEvent(pointId, cooldown));
        }

        public IObservable<ActivatePointEvent> GetEventFlow() => eventsSubject;
    }
}
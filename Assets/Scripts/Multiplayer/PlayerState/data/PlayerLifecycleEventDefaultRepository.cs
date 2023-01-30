using System;
using Multiplayer.PlayerState.domain.model;
using Multiplayer.PlayerState.domain.repositories;
using UniRx;

namespace Multiplayer.PlayerState.data
{
    internal class PlayerLifecycleEventDefaultRepository: IPlayerLifecycleEventRepository
    {
        private readonly Subject<PlayerLifecycleEvent> eventSubject = new();
        public IObservable<PlayerLifecycleEvent> GetLifecycleEvents() => eventSubject;
        public void SendLifecycleEvent(PlayerLifecycleEvent lifecycleEvent) => eventSubject.OnNext(lifecycleEvent);
    }
}
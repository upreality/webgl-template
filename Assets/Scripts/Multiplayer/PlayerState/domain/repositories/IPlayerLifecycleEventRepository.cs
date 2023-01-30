using System;
using Multiplayer.PlayerState.domain.model;

namespace Multiplayer.PlayerState.domain.repositories
{
    public interface IPlayerLifecycleEventRepository
    {
        public IObservable<PlayerLifecycleEvent> GetLifecycleEvents();
        public void SendLifecycleEvent(PlayerLifecycleEvent lifecycleEvent);
    }
}
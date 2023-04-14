using System;

namespace Multiplayer.PlayerManagement.domain
{
    public interface IPlayerConnectionEventsRepository
    {
        public IObservable<PlayerConnectionEvent> GetConnectionEvents();
    }

    public struct PlayerConnectionEvent
    {
        public string PlayerId;
        public bool Connected;

        public PlayerConnectionEvent(string playerId, bool connected)
        {
            PlayerId = playerId;
            Connected = connected;
        }
    }
}
using System;
using System.Collections.Generic;

namespace Multiplayer.PlayerManagement.domain
{
    public interface IPlayerIdsRepository
    {
        public void Add(string playerID);
        public void Remove(string playerID);
        public ISet<string> GetConnectedPlayers();
        public bool GetConnectedState(string playerID);
        public IObservable<bool> GetConnectedStateFlow(string playerID);
    }
}
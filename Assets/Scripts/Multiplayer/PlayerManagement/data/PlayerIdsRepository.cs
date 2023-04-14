using System;
using System.Collections.Generic;
using Mirror;
using Multiplayer.PlayerManagement.domain;
using UniRx;
using Utils.Reactive;

namespace Multiplayer.PlayerManagement.data
{
    public class PlayerIdsRepository : NetworkBehaviour, IPlayerIdsRepository, IPlayerConnectionEventsRepository
    {
        // ReSharper disable once MemberCanBePrivate.Global
        public readonly SyncHashSet<string> PlayerIds = new();

        private readonly ReactiveDictionary<string, bool> connectedStates = new();

        public override void OnStartServer() => PlayerIds.Callback += OnPlayerIdsChanged;
        public override void OnStopServer() => PlayerIds.Callback -= OnPlayerIdsChanged;
        public override void OnStartClient() => PlayerIds.Callback += OnPlayerIdsChanged;
        public override void OnStopClient() => PlayerIds.Callback -= OnPlayerIdsChanged;


        public void Add(string playerID)
        {
            if (!isServer) return;
            PlayerIds.Add(playerID);
        }

        public void Remove(string playerID)
        {
            if (!isServer) return;
            PlayerIds.Remove(playerID);
        }

        public ISet<string> GetConnectedPlayers() => PlayerIds;

        public bool GetConnectedState(string playerID) => PlayerIds.Contains(playerID);

        public IObservable<bool> GetConnectedStateFlow(string playerID)
        {
            var currentState = GetConnectedState(playerID);
            return connectedStates.GetItemFlow(playerID).StartWith(currentState).DistinctUntilChanged();
        }

        public IObservable<PlayerConnectionEvent> GetConnectionEvents()
        {
            var addEvents = connectedStates
                .ObserveAdd()
                .Select(addEvent => new PlayerConnectionEvent(addEvent.Key, addEvent.Value))
                .DistinctUntilChanged();
            var replaceEvents = connectedStates
                .ObserveReplace()
                .Select(replaceEvent => new PlayerConnectionEvent(replaceEvent.Key, replaceEvent.NewValue))
                .DistinctUntilChanged();
            var removeEvents = connectedStates
                .ObserveRemove()
                .Select(removeEvent => new PlayerConnectionEvent(removeEvent.Key, false))
                .DistinctUntilChanged();
            return addEvents
                .Merge(replaceEvents, removeEvents)
                .DistinctUntilChanged();
        }

        private void OnPlayerIdsChanged(SyncSet<string>.Operation op, string playerId)
        {
            connectedStates[playerId] = op == SyncSet<string>.Operation.OP_ADD;
        }
    }
}
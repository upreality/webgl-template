using System;
using Multiplayer.MatchState.domain.model;

namespace Multiplayer.MatchState.domain.repositories
{
    public interface IMatchStateRepository
    {
        internal void SetMatchState(MatchStates state);
        public IObservable<MatchStates> GetMatchStateFlow();
        MatchStates GetMatchState();
    }
}
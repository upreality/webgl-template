using System;
using Multiplayer.MatchState.domain.model;
using Multiplayer.MatchState.domain.repositories;
using UniRx;

namespace Multiplayer.MatchState.data
{
    internal class MatchStateDefaultRepository : IMatchStateRepository
    {
        private readonly BehaviorSubject<MatchStates> matchStateSubject = new(MatchStates.None);
        
        void IMatchStateRepository.SetMatchState(MatchStates state)
        {
            if (matchStateSubject.Value == state) return;
            matchStateSubject.OnNext(state);
        }

        public IObservable<MatchStates> GetMatchStateFlow() => matchStateSubject;
        public MatchStates GetMatchState() => matchStateSubject.Value;
    }
}
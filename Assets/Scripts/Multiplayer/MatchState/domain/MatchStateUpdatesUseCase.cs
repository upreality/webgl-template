using System;
using Multiplayer.MatchState.domain.model;
using Multiplayer.MatchState.domain.repositories;
using Multiplayer.MatchTimer.domain;
using UniRx;
using Zenject;

namespace Multiplayer.MatchState.domain
{
    internal class MatchStateUpdatesUseCase
    {
        [Inject] private TimerCompletedEventUseCase timerCompletedEventUseCase;
        [Inject] private IMatchStateRepository stateRepository;
        [Inject] private NextMatchStateUseCase nextMatchStateUseCase;

        public IObservable<MatchStates> GetUpdatesFlow() => timerCompletedEventUseCase
            .GetTimerCompletedEventFlow()
            .Select(_ => GetNextMatchState());

        private MatchStates GetNextMatchState()
        {
            var currentState = stateRepository.GetMatchState();
            return nextMatchStateUseCase.GetNextMatchState(currentState);
        }
    }
}
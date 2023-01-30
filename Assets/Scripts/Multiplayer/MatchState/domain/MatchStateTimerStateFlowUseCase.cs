using System;
using Multiplayer.MatchState.domain.model;
using Multiplayer.MatchState.domain.repositories;
using Multiplayer.MatchTimer.domain.repositories;
using UniRx;
using Zenject;

namespace Multiplayer.MatchState.domain
{
    public class MatchStateTimerStateFlowUseCase
    {
        [Inject] private IMatchStateRepository matchStateRepository;
        [Inject] private IMatchTimerRepository matchTimerRepository;

        public IObservable<TimerState> GetTimerStateFlow(MatchStates state)
        {
            var correctMatchStateFlow = matchStateRepository
                .GetMatchStateFlow()
                .Select(currentState => currentState == state);
            return matchTimerRepository.GetMatchTimeSecondsFlow()
                .WithLatestFrom(correctMatchStateFlow, CreateTimerState)
                .DistinctUntilChanged();
        }

        private static TimerState CreateTimerState(int timeLeft, bool isCorrectState)
        {
            var time = isCorrectState ? timeLeft : 0;
            return new TimerState(isCorrectState, time);
        }
    }
}
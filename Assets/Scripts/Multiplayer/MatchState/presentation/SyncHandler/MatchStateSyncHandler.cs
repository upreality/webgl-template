using System;
using Multiplayer.MatchState.domain;
using Multiplayer.MatchState.domain.model;
using Multiplayer.MatchState.domain.repositories;
using UniRx;
using Utils.MirrorCodegen;
using Zenject;

namespace Multiplayer.MatchState.presentation.SyncHandler
{
    [ServerSubscriptionHandler]
    public class MatchStateSyncHandler
    {
        [Inject] private IMatchStateRepository matchStateRepository;
        [Inject] private IMatchStateTimerRepository matchStateTimerRepository;
        [Inject] private IMatchStateDurationRepository matchStateDurationRepository;
        [Inject] private NextMatchStateUseCase nextMatchStateUseCase;

        [ServerSubscription]
        public IDisposable HandleMatchState()
        {
            matchStateRepository.SetMatchState(MatchStates.None);
            return StartNextState().Subscribe();
        }

        private IObservable<Unit> StartNextState()
        {
            var currentState = matchStateRepository.GetMatchState();
            var nextState = nextMatchStateUseCase.GetNextMatchState(currentState);
            matchStateRepository.SetMatchState(nextState);

            var stateDuration = matchStateDurationRepository
                .GetStateDuration(nextState, out var definedDuration)
                ? definedDuration
                : 1;
            var span = TimeSpan.FromSeconds(1);
            matchStateTimerRepository.SetTimer(stateDuration);
            return Observable
                .Interval(span)
                .Select(i => stateDuration - i)
                .Where(timeLeft => timeLeft >= 0)
                .Do(matchStateTimerRepository.SetTimer)
                .Where(timeLeft => timeLeft == 0)
                .Select(_ => StartNextState())
                .Switch()
                .AsUnitObservable();
        }
    }
}
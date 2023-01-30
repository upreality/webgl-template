using System;
using Multiplayer.MatchState.domain.model;
using Multiplayer.MatchState.domain.repositories;
using Multiplayer.MatchTimer.domain.repositories;
using Zenject;

namespace Multiplayer.MatchState.domain
{
    public class StartMatchStateUseCase
    {
        [Inject] private IMatchStateDurationRepository matchStateDurationRepository;
        [Inject] private IMatchStateRepository matchStateRepository;
        [Inject] private IMatchTimerRepository matchTimerRepository;

        public void StartMatchState(MatchStates state, int timeLeft = 0)
        {
            matchStateRepository.SetMatchState(state);
            matchTimerRepository.StopTimer();

            timeLeft = Math.Max(0, timeLeft);
            if (!matchStateDurationRepository.GetStateDuration(state, out var duration)) return;
            matchTimerRepository.StartTimer(duration - timeLeft);
        }
    }
}
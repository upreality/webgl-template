using System;

namespace Multiplayer.MatchTimer.domain.repositories
{
    public interface IMatchTimerRepository
    {
        public IObservable<int> GetMatchTimeSecondsFlow();
        public void StartTimer(int seconds);
        public void StopTimer();
    }
}
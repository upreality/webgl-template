using System;
using JetBrains.Annotations;
using Multiplayer.MatchTimer.domain.repositories;
using UniRx;

namespace Multiplayer.MatchTimer.data
{
    public class MatchTimerDefaultRepository : IMatchTimerRepository
    {
        [CanBeNull] private IDisposable timerSubscription;
        private readonly BehaviorSubject<int> currentTimer = new(0);

        public IObservable<int> GetMatchTimeSecondsFlow() => currentTimer;

        void IMatchTimerRepository.StartTimer(int seconds)
        {
            timerSubscription?.Dispose();
            if (seconds < 0) return;
            currentTimer.OnNext(seconds);
            timerSubscription = Observable
                .Timer(TimeSpan.FromSeconds(1))
                .Repeat()
                .Where(_ => currentTimer.Value > 0)
                .Subscribe(_ =>
                    currentTimer.OnNext(currentTimer.Value - 1)
                );
        }

        ~MatchTimerDefaultRepository()
        {
            timerSubscription?.Dispose();
        }

        void IMatchTimerRepository.StopTimer()
        {
            timerSubscription?.Dispose();
            timerSubscription = null;
        }
    }
}
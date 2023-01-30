using System;
using Multiplayer.MatchState.domain.model;
using Multiplayer.MatchState.domain.repositories;
using Multiplayer.SpawnPoints.domain.repositories;
using UniRx;
using Zenject;

namespace Multiplayer.SpawnPoints.domain
{
    public class SpawnPointCooldownUseCase
    {
        [Inject] private IMatchStateRepository matchStateRepository;
        [Inject] private ISpawnPointEventsRepository cooldownEventRepository;
        
        public IObservable<int> GetCooldownFlow(int pointId) => matchStateRepository
            .GetMatchStateFlow()
            .Select(matchState => GetCooldownFlow(matchState, pointId))
            .Switch();

        private IObservable<int> GetCooldownFlow(MatchStates matchState, int pointId)
        {
            if (matchState != MatchStates.Playing) return Observable.Return(0);
            var setCooldownFlow = cooldownEventRepository
                .GetEventFlow()
                .Where(cooldownEvent => cooldownEvent.PointId == pointId)
                .Select(cooldownEvent => cooldownEvent.Cooldown);

            return setCooldownFlow.Select(CreateCountdown).Switch();
        }

        private static IObservable<int> CreateCountdown(int seconds) => Observable.Create<int>(observer =>
        {
            if (seconds == 0)
            {
                observer.OnNext(0);
                observer.OnCompleted();
                return Disposable.Empty;
            }

            var span = TimeSpan.FromSeconds(1);
            return Observable
                .Timer(span)
                .Repeat()
                .Select(_ => --seconds)
                .StartWith(seconds)
                .TakeWhile(cooldownLeft => cooldownLeft >= 0)
                .Subscribe(cooldownLeft =>
                {
                    observer.OnNext(cooldownLeft);
                    if (cooldownLeft != 0) return;
                    observer.OnCompleted();
                });
        });
    }
}
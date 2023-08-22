using System;
using HNS.domain.model;
using HNS.domain.repositories;
using UniRx;
using Zenject;

namespace HNS.domain
{
    public class HiderStateUseCase
    {
        [Inject] private HNSPlayerSnapshotsUseCase playerSnapshotsUseCase;
        [Inject] private CatcherHandsUseCase hands;
        [Inject] private HNSGameStateUseCase gameStateUseCase;
        [Inject] private ISleepPlacesRepository sleepPlaces;

        private IObservable<GameStates> GameStateFlow => gameStateUseCase.GetStateFlow();

        public IObservable<HiderState> GetStateFlow(long playerId) => GetSnapshotFlow(playerId)
            .CombineLatest(GameStateFlow, GetHiderState)
            .Switch();

        private IObservable<HiderSnapshotItem> GetSnapshotFlow(long playerId) => playerSnapshotsUseCase
            .GetHider(playerId, out var flow)
            ? flow
            : Observable.Empty<HiderSnapshotItem>();

        private IObservable<HiderState> GetCatchedHiderState(
            HiderSnapshotItem item,
            long catcherId
        ) => hands
            .Get(catcherId)
            .Select(handsPos => new HiderState
                {
                    Transform = handsPos.GetSnapshot(),
                    Character = item.character,
                    Behavior = HiderBehavior.Catched
                }
            );

        private IObservable<HiderState> GetSleepingHiderState(
            HiderSnapshotItem item,
            long sleepPlaceId
        ) {
            var sleepPos = sleepPlaces.Get(sleepPlaceId);
            var state = new HiderState
            {
                Transform = sleepPos,
                Character = item.character,
                Behavior = HiderBehavior.Layed
            };
            return Observable.Return(state);
        }

        private static IObservable<HiderState> GetHiderState(HiderSnapshotItem item, HiderBehavior behavior)
        {
            var state = new HiderState
            {
                Transform = item.transform,
                Character = item.character,
                Behavior = behavior
            };
            return Observable.Return(state);
        }

        private IObservable<HiderState> GetHiderState(HiderSnapshotItem item, GameStates state)
        {
            if (state == GameStates.Pending)
                return GetHiderState(item, HiderBehavior.Pending);

            var catcherId = item.catcherId;
            if (catcherId.HasValue)
                return GetCatchedHiderState(item, catcherId.Value);

            var sleepPlaceId = item.sleepPlaceId;
            return sleepPlaceId.HasValue
                ? GetSleepingHiderState(item, sleepPlaceId.Value) 
                : GetHiderState(item, HiderBehavior.Hiding);
        }
    }
}
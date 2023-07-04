using System;
using HNS.data;
using HNS.Model;
using HNS.Player;
using UniRx;
using Zenject;

namespace HNS.domain
{
    public class HiderStateUseCase
    {
        [Inject] private HNSSnapshotRepository snapshots;
        [Inject] private CatcherHandsRepository hands;
        [Inject] private HNSGameStateRepository states;
        [Inject] private ISleepPlacesRepository sleepPlaces;

        private IObservable<GameStates> GameStateFlow => states.GetStateFlow();

        public IObservable<HiderState> GetStateFlow(long playerId) => GetSnapshotFlow(playerId)
            .CombineLatest(GameStateFlow, GetHiderState)
            .Switch();

        private IObservable<HiderSnapshotItem> GetSnapshotFlow(long playerId) => snapshots
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
            if (catcherId != null)
                return GetCatchedHiderState(item, catcherId.Value);

            var sleepPlaceId = item.sleepPlaceId;
            if (sleepPlaceId != null)
                return GetSleepingHiderState(item, sleepPlaceId.Value);
            
            return GetHiderState(item, HiderBehavior.Hiding);
        }
    }
}
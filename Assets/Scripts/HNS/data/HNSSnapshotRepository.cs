using System;
using HNS.domain;
using HNS.Model;

namespace HNS.data
{
    public class HNSSnapshotRepository
    {
        private ReactiveCompletableItemsMap<long, SeekerSnapshotItem> seekers = new();
        private ReactiveCompletableItemsMap<long, HiderSnapshotItem> hiders = new();

        public void Submit(long userId, SeekerSnapshotItem seeker) => seekers.Set(userId, seeker);

        public void Submit(long userId, HiderSnapshotItem hider) => hiders.Set(userId, hider);

        public bool GetSeeker(long userId, out IObservable<SeekerSnapshotItem> snapshotFlow) => seekers.Get(userId, out snapshotFlow);
        
        public bool GetHider(long userId, out IObservable<HiderSnapshotItem> snapshotFlow) => hiders.Get(userId, out snapshotFlow);

        public void Remove(long userId)
        {
            seekers.Remove(userId);
            hiders.Remove(userId);
        }

        public void Clear()
        {
            seekers.Clear();
            hiders.Clear();
        }
    }
}
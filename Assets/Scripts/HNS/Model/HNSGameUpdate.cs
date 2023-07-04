using System;
using System.Collections.Generic;
using System.Linq;

namespace HNS.Model
{
    [Serializable]
    public struct HNSGameUpdate
    {
        public HNSGameSnapshot snapshot;
        public bool isFinished;
        public FinishReason? finishReason;
    }

    [Serializable]
    public class PairData<K, T>
    {
        public K first;
        public T second;
    }

    [Serializable]
    public struct HNSGameSnapshot
    {
        public List<PairData<long, SeekerSnapshotItem>> seekers;
        public List<PairData<long, HiderSnapshotItem>> hiders;

        private Dictionary<long, SeekerSnapshotItem> seekersDictionary;

        public Dictionary<long, SeekerSnapshotItem> Seekers => seekersDictionary
            ??= seekers.ToDictionary(pair => pair.first, pair => pair.second);

        private Dictionary<long, HiderSnapshotItem> hidersDictionary;

        public Dictionary<long, HiderSnapshotItem> Hiders => hidersDictionary
            ??= hiders.ToDictionary(pair => pair.first, pair => pair.second);
    }

    [Serializable]
    public struct HiderSnapshotItem
    {
        public TransformSnapshot transform;
        public Character character;
        public int beenCatched;
        public int beenLayed;
        public long? catcherId;
        public long? sleepPlaceId;
    }

    [Serializable]
    public struct SeekerSnapshotItem
    {
        public TransformSnapshot transform;
        public Character character;
        public int catched;
        public int layed;
        public long? catchedHiderId;
    }

    public enum FinishReason
    {
        UsersLeft,
        Aborted,
        TimeLeft,
        AllHidersSleeping,
    }
}
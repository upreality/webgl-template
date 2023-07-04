using System;
using HNS.domain;
using UnityEngine;

namespace HNS.data
{
    public class CatcherHandsRepository
    {
        private readonly ReactiveCompletableItemsMap<long, Vector3> hands = new();

        public void Set(long catcherId, Vector3 handsPos) => hands.Set(catcherId, handsPos);

        public void Remove(long catcherId) => hands.Remove(catcherId);

        public IObservable<Vector3> Get(long catcherId) => hands.GetInFuture(catcherId);
    }
}
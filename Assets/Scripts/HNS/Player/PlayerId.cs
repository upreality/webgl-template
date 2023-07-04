using System;
using UniRx;
using UnityEngine;

namespace HNS.Player
{
    public class PlayerIdProvider: MonoBehaviour
    {
        private readonly BehaviorSubject<long> playerIdSubject = new(-1);

        public void SetPlayerId(long id) => playerIdSubject.OnNext(id);

        public IObservable<long> PlayerIdFlow => playerIdSubject
            .Where(id => id > 0);
    }
}
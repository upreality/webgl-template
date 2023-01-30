using System;
using Multiplayer.Ammo.domain.repository;
using UniRx;

namespace Multiplayer.Ammo.data
{
    public class AmmoDefaultRepository: IAmmoRepository
    {
        private readonly BehaviorSubject<int> ammoSubject = new(0);

        public int GetLoadedAmmoCount() => ammoSubject.Value;

        public IObservable<int> GetLoadedAmmoCountFlow() => ammoSubject;

        public void SetLoadedAmmo(int count) => ammoSubject.OnNext(count);
    }
}
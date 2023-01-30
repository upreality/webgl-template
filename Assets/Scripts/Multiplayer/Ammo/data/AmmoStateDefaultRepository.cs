using System;
using Multiplayer.Ammo.domain.model;
using Multiplayer.Ammo.domain.repository;
using UniRx;

namespace Multiplayer.Ammo.data
{
    public class AmmoStateDefaultRepository : IAmmoStateRepository
    {
        private readonly BehaviorSubject<AmmoState> ammoStateSubject = new(AmmoState.Empty);

        public AmmoState GetAmmoState() => ammoStateSubject.Value;
        public IObservable<AmmoState> GetAmmoStateFlow() => ammoStateSubject;
        public void SetAmmoState(AmmoState state) => ammoStateSubject.OnNext(state);
    }
}
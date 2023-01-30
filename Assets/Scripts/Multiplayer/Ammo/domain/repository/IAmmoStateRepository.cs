using System;
using Multiplayer.Ammo.domain.model;

namespace Multiplayer.Ammo.domain.repository
{
    public interface IAmmoStateRepository
    {
        public AmmoState GetAmmoState();
        public IObservable<AmmoState> GetAmmoStateFlow();
        public void SetAmmoState(AmmoState state);
    }
}
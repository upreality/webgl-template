using System;

namespace Multiplayer.Ammo.domain.repository
{
    public interface IAmmoRepository
    {
        public int GetLoadedAmmoCount();
        public IObservable<int> GetLoadedAmmoCountFlow();
        public void SetLoadedAmmo(int count);
    }
}
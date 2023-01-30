using System;
using Multiplayer.Weapons.domain.model;

namespace Multiplayer.Weapons.domain.repositories
{
    public interface ISelectedWeaponRepository
    {
        bool GetSelectedWeapon(out Weapon weapon);
        IObservable<Weapon> GetSelectedWeaponFlow();
        void SetSelectedWeapon(long weaponId);
    }
}
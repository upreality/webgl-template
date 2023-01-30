using System.Collections.Generic;
using Multiplayer.Weapons.domain.model;

namespace Multiplayer.Weapons.domain.repositories
{
    public interface IWeaponsRepository
    {
        public Weapon GetWeapon(long weaponId);
        public List<Weapon> GetAvailableWeapons();
    }
}
using System.Collections.Generic;
using Multiplayer.Weapons.data.models;

namespace Multiplayer.Weapons.data.dao
{
    public interface IWeaponEntitiesDao
    {
        public List<WeaponEntity> GetWeaponEntities();
        public WeaponEntity GetWeaponEntity(int number);
    }
}
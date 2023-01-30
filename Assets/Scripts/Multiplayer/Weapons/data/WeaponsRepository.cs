using System.Collections.Generic;
using System.Linq;
using Multiplayer.Weapons.data.dao;
using Multiplayer.Weapons.data.models;
using Multiplayer.Weapons.domain.model;
using Multiplayer.Weapons.domain.repositories;
using Zenject;

namespace Multiplayer.Weapons.data
{
    public class WeaponsRepository : IWeaponsRepository
    {
        [Inject] private IWeaponEntitiesDao weaponEntitiesDao;

        public Weapon GetWeapon(long weaponId)
        {
            var entity = weaponEntitiesDao.GetWeaponEntity((int) weaponId);
            return ConvertToWeapon(entity, weaponId);
        }

        public List<Weapon> GetAvailableWeapons() => weaponEntitiesDao
            .GetWeaponEntities()
            .Select((entity, index) =>
                ConvertToWeapon(entity, index)
            ).ToList();

        private static Weapon ConvertToWeapon(WeaponEntity entity, long weaponId) => new(
            weaponId,
            entity.Name,
            entity.AmmoCapacity,
            entity.PerMinuteRate,
            entity.Type
        );
    }
}
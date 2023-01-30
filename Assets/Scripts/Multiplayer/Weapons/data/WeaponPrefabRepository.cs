using Multiplayer.Weapons.data.dao;
using Multiplayer.Weapons.domain.repositories;
using UnityEngine;
using Zenject;

namespace Multiplayer.Weapons.data
{
    public class WeaponPrefabRepository: IWeaponPrefabRepository
    {
        [Inject] private IWeaponEntitiesDao weaponEntitiesDao;
        
        public GameObject GetWeaponPrefab(long weaponId) => weaponEntitiesDao.GetWeaponEntity((int) weaponId).prefab;
    }
}
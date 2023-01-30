using UnityEngine;

namespace Multiplayer.Weapons.domain.repositories
{
    public interface IWeaponPrefabRepository
    {
        public GameObject GetWeaponPrefab(long weaponId);
    }
}
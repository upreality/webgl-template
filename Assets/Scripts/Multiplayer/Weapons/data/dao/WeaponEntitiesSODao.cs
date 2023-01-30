using System;
using System.Collections.Generic;
using Multiplayer.Weapons.data.models;
using UnityEngine;

namespace Multiplayer.Weapons.data.dao
{
    // ReSharper disable once InconsistentNaming
    [CreateAssetMenu(menuName = "Weapons/WeaponsListDao")]
    public class WeaponEntitiesSODao : ScriptableObject, IWeaponEntitiesDao
    {
        [SerializeField] private List<WeaponEntity> entities = new List<WeaponEntity>();
        
        public List<WeaponEntity> GetWeaponEntities() => entities;

        public WeaponEntity GetWeaponEntity(int number) => entities[Math.Max(0, number - 1)];

    }
}
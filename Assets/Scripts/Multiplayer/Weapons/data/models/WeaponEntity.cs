using System;
using Multiplayer.Weapons.domain.model;
using UnityEngine;

namespace Multiplayer.Weapons.data.models
{
    [Serializable]
    public class WeaponEntity
    {
        public string Name;
        public int AmmoCapacity;
        public int PerMinuteRate;
        public Weapon.DamageType Type;
        public Sprite preview;
        public GameObject prefab;
    }
}
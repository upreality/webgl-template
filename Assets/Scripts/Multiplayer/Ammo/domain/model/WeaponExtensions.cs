using Multiplayer.Weapons.domain.model;

namespace Multiplayer.Ammo.domain.model
{
    public static class WeaponExtensions
    {
        public static bool IsAmmoAvailable(this Weapon weapon)
        {
            return weapon.Type == Weapon.DamageType.Ranged;
        }
    }
}
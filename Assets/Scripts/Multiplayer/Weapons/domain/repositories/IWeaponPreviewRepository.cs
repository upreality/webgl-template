using UnityEngine;

namespace Multiplayer.Weapons.domain.repositories
{
    public interface IWeaponPreviewRepository
    {
        public Sprite GetWeaponPreview(long weaponId);
    }
}
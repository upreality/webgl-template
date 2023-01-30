using Multiplayer.Weapons.data.dao;
using Multiplayer.Weapons.domain.repositories;
using UnityEngine;
using Zenject;

namespace Multiplayer.Weapons.data
{
    public class WeaponPreviewRepository: IWeaponPreviewRepository
    {
        
        [Inject] private IWeaponEntitiesDao weaponEntitiesDao;
        
        public Sprite GetWeaponPreview(long weaponId) => weaponEntitiesDao.GetWeaponEntity((int) weaponId).preview;
    }
}
using Multiplayer.Ammo.domain.model;
using Multiplayer.Ammo.domain.repository;
using Multiplayer.Weapons.domain.repositories;
using Zenject;
using static Multiplayer.Ammo.domain.ReloadAmmoUseCase.ReloadAmmoResult;

namespace Multiplayer.Ammo.domain
{
    public class ReloadAmmoUseCase
    {
        [Inject] private IAmmoRepository ammoRepository;
        [Inject] private IAmmoStateRepository ammoStateRepository;
        [Inject] private ISelectedWeaponRepository selectedWeaponRepository;

        public ReloadAmmoResult ReloadAmmo()
        {
            if (!selectedWeaponRepository.GetSelectedWeapon(out var selectedWeapon)) return NoWeapon;
            if (!selectedWeapon.IsAmmoAvailable()) return NotReloadable;
            if (selectedWeapon.AmmoCapacity == ammoRepository.GetLoadedAmmoCount()) return FullAmmo;
            ammoRepository.SetLoadedAmmo(selectedWeapon.AmmoCapacity);
            ammoStateRepository.SetAmmoState(AmmoState.Full);
            return Success;
        }

        public enum ReloadAmmoResult
        {
            Success,
            FullAmmo,
            NotReloadable,
            NoWeapon
        }
    }
}
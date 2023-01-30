using Multiplayer.Ammo.domain.model;
using Multiplayer.Ammo.domain.repository;
using Multiplayer.Weapons.presentation;
using Zenject;

namespace Multiplayer.Ammo.presentation
{
    public class ReloadWeaponSelectionAvailableProvider: WeaponSelectionController.IWeaponSelectionAvailableProvider
    {
        [Inject] private IAmmoStateRepository ammoStateRepository;
        public bool IsSelectionAvailable() => ammoStateRepository.GetAmmoState() != AmmoState.Reloading;
    }
}
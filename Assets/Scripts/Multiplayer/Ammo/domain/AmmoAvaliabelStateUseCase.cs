using System;
using Multiplayer.Ammo.domain.model;
using Multiplayer.Weapons.domain.repositories;
using UniRx;
using Zenject;
using static Multiplayer.Weapons.domain.model.Weapon.DamageType;

namespace Multiplayer.Ammo.domain
{
    public class AmmoAvailableStateUseCase
    {
        [Inject] private ISelectedWeaponRepository selectedWeaponRepository;

        public IObservable<bool> GetAmmoAvailableStateFlow() => selectedWeaponRepository
            .GetSelectedWeaponFlow()
            .Select(weapon => weapon.Type != Melee);

        public bool GetAmmoAvailableState()
        {
            return selectedWeaponRepository.GetSelectedWeapon(out var weapon) && weapon.IsAmmoAvailable();
        }
    }
}
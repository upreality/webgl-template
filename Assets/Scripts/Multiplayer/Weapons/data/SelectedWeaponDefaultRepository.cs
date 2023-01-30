using System;
using Multiplayer.Weapons.domain;
using Multiplayer.Weapons.domain.model;
using Multiplayer.Weapons.domain.repositories;
using UniRx;
using Zenject;

namespace Multiplayer.Weapons.data
{
    public class SelectedWeaponDefaultRepository : ISelectedWeaponRepository
    {
        private readonly IWeaponsRepository weaponsRepository;
        private const long UndefinedWeaponId = -1L;
        private readonly BehaviorSubject<long> selectedWeaponIdSubject;

        [Inject]
        public SelectedWeaponDefaultRepository(
            IWeaponsRepository weaponsRepository,
            DefaultWeaponUseCase defaultWeaponUseCase
        )
        {
            this.weaponsRepository = weaponsRepository;
            selectedWeaponIdSubject = new BehaviorSubject<long>(defaultWeaponUseCase.GetDefaultWeapon().ID);
        }

        public bool GetSelectedWeapon(out Weapon weapon)
        {
            if (selectedWeaponIdSubject.Value == UndefinedWeaponId)
            {
                weapon = null;
                return false;
            }

            weapon = weaponsRepository.GetWeapon(selectedWeaponIdSubject.Value);
            return true;
        }

        public IObservable<Weapon> GetSelectedWeaponFlow() => selectedWeaponIdSubject
            .Where(id => id != UndefinedWeaponId)
            .Select(weaponsRepository.GetWeapon);

        public void SetSelectedWeapon(long weaponId) => selectedWeaponIdSubject.OnNext(weaponId);
    }
}
using Multiplayer.Weapons.domain.model;
using Multiplayer.Weapons.domain.repositories;
using Zenject;

namespace Multiplayer.Weapons.domain
{
    public class SelectWeaponUseCase
    {
        [Inject] private ISelectedWeaponRepository selectedWeaponRepository;
        [Inject] private IWeaponsRepository weaponsRepository;

        public SelectWeaponResult SelectWeapon(int number)
        {
            var weapons = weaponsRepository.GetAvailableWeapons();
            if (weapons.Count == 0) return SelectWeaponResult.Failure;
            var selectionWeaponIndex = number - 1;
            if (selectionWeaponIndex < 0 || selectionWeaponIndex >= weapons.Count) return SelectWeaponResult.Failure;
            
            selectedWeaponRepository.SetSelectedWeapon(weapons[selectionWeaponIndex].ID);
            return SelectWeaponResult.Success;
        }
    }
}
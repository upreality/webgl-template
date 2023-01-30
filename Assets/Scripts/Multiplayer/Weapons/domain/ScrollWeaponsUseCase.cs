using System.Linq;
using Multiplayer.Weapons.domain.model;
using Multiplayer.Weapons.domain.repositories;
using Zenject;

namespace Multiplayer.Weapons.domain
{
    public class ScrollWeaponsUseCase
    {
        [Inject] private ISelectedWeaponRepository selectedWeaponRepository;
        [Inject] private IWeaponsRepository weaponsRepository;

        public SelectWeaponResult ScrollWeapon(ScrollDirection direction)
        {
            var weapons = weaponsRepository.GetAvailableWeapons();
            if (weapons.Count == 0 || !selectedWeaponRepository.GetSelectedWeapon(out var selectedWeapon))
                return SelectWeaponResult.Failure;

            int scrolledToWeaponIndex;
            if (weapons.All(weapon => weapon.ID != selectedWeapon.ID))
            {
                scrolledToWeaponIndex = (direction == ScrollDirection.Next) ? 0 : weapons.Count - 1;
            }
            else
            {
                var weaponIndex = weapons.FindIndex(weapon => weapon.ID == selectedWeapon.ID);
                scrolledToWeaponIndex = (direction == ScrollDirection.Next) ? weaponIndex + 1 : weaponIndex - 1;
                scrolledToWeaponIndex %= weapons.Count;
                if (scrolledToWeaponIndex < 0)
                    scrolledToWeaponIndex += weapons.Count;
            }

            selectedWeaponRepository.SetSelectedWeapon(weapons[scrolledToWeaponIndex].ID);
            return SelectWeaponResult.Success;
        }

        public enum ScrollDirection
        {
            Next,
            Previous
        }
    }
}
using System.Linq;
using Multiplayer.Weapons.domain.model;
using Multiplayer.Weapons.domain.repositories;
using Zenject;

namespace Multiplayer.Weapons.domain
{
    public class DefaultWeaponUseCase
    {
        [Inject] private IWeaponsRepository weaponsRepository;

        public Weapon GetDefaultWeapon() => weaponsRepository.GetAvailableWeapons().First();
    }
}
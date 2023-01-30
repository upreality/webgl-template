using Multiplayer.Ammo.domain.model;
using Multiplayer.Ammo.domain.repository;
using Zenject;

namespace Multiplayer.Ammo.domain
{
    public class PassAmmoUseCase
    {
        [Inject] private IAmmoRepository ammoRepository;
        [Inject] private IAmmoStateRepository ammoStateRepository;

        public PassAmmoResult Pass(int amount)
        {
            var isReloading = ammoStateRepository.GetAmmoState() == AmmoState.Reloading;
            if (isReloading)
                return PassAmmoResult.Reloading;

            var count = ammoRepository.GetLoadedAmmoCount() - amount;
            if (count < 0)
                return PassAmmoResult.NotEnough;

            ammoRepository.SetLoadedAmmo(count);

            if (count == 0)
                ammoStateRepository.SetAmmoState(AmmoState.Empty);
            else
                ammoStateRepository.SetAmmoState(AmmoState.Loaded);

            return PassAmmoResult.Success;
        }

        public enum PassAmmoResult
        {
            Success,
            NotEnough,
            Reloading
        }
    }
}
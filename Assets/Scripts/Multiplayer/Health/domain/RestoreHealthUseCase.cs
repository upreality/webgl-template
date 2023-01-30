using Multiplayer.Health.domain.repositories;
using Zenject;

namespace Multiplayer.Health.domain
{
    public class RestoreHealthUseCase
    {
        [Inject] private IPlayerHealthRepository playerHealthRepository;
        [Inject] private IMaxHealthRepository maxHealthRepository;

        public void RestoreHealth()
        {
            var maxHealth = maxHealthRepository.GetMaxHealth();
            playerHealthRepository.SetHealth(maxHealth);
        }
    }
}
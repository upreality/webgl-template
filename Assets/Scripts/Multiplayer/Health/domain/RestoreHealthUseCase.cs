using Multiplayer.Health.domain.repositories;
using Zenject;

namespace Multiplayer.Health.domain
{
    public class RestoreHealthUseCase
    {
        [Inject] private IHealthHandlersRepository healthHandlersRepository;
        [Inject] private IMaxHealthRepository maxHealthRepository;

        public void RestoreHealth(string handlerId)
        {
            var maxHealth = maxHealthRepository.GetMaxHealth();
            healthHandlersRepository.SetHealth(handlerId, maxHealth);
        }
    }
}
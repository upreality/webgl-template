using System;
using Multiplayer.Health.domain.model;
using Multiplayer.Health.domain.repositories;
using Zenject;

namespace Multiplayer.Health.domain
{
    public class IncreaseHealthUseCase
    {
        [Inject] private IHealthHandlersRepository healthHandlersRepository;
        [Inject] private IMaxHealthRepository maxHealthRepository;
        
        public IncreaseHealthResult IncreaseHealth(string playerId, int amount)
        {
            var currentHealth = healthHandlersRepository.GetHealth(playerId);
            var maxHealth = maxHealthRepository.GetMaxHealth();
            if (currentHealth >= maxHealth)
                return IncreaseHealthResult.AlreadyFull;

            currentHealth = Math.Min(maxHealth, currentHealth + amount);
            healthHandlersRepository.SetHealth(playerId, currentHealth);
            return currentHealth < maxHealth ? IncreaseHealthResult.Increased : IncreaseHealthResult.HealthFilledUp;
        }
    }
}
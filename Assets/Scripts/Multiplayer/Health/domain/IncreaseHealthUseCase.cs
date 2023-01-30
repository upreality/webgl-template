using System;
using Multiplayer.Health.domain.model;
using Multiplayer.Health.domain.repositories;
using Zenject;

namespace Multiplayer.Health.domain
{
    public class IncreaseHealthUseCase
    {
        [Inject] private IPlayerHealthRepository playerHealthRepository;
        [Inject] private IMaxHealthRepository maxHealthRepository;
        
        public IncreaseHealthResult IncreaseHealth(int amount)
        {
            var currentHealth = playerHealthRepository.GetHealth();
            var maxHealth = maxHealthRepository.GetMaxHealth();
            if (currentHealth >= maxHealth)
                return IncreaseHealthResult.AlreadyFull;

            currentHealth = Math.Min(maxHealth, currentHealth + amount);
            playerHealthRepository.SetHealth(currentHealth);
            return currentHealth < maxHealth ? IncreaseHealthResult.Increased : IncreaseHealthResult.HealthFilledUp;
        }
    }
}
using System;
using Multiplayer.Health.domain.model;
using Multiplayer.Health.domain.repositories;
using Zenject;

namespace Multiplayer.Health.domain
{
    public class DecreaseHealthUseCase
    {
        [Inject] private IPlayerHealthRepository playerHealthRepository;
        public DecreaseHealthResult DecreaseHealth(int amount)
        {
            var currentHealth = playerHealthRepository.GetHealth();
            if (currentHealth <= 0)
                return DecreaseHealthResult.NoHealth;

            currentHealth = Math.Max(0, currentHealth - amount);
            playerHealthRepository.SetHealth(currentHealth);
            return currentHealth > 0 ? DecreaseHealthResult.Decreased : DecreaseHealthResult.HealthRanOut;
        }
    }
}
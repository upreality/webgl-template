using System;
using Multiplayer.Health.domain.model;
using Multiplayer.Health.domain.repositories;
using Zenject;

namespace Multiplayer.Health.domain
{
    public class DecreaseHealthUseCase
    {
        [Inject] private IHealthHandlersRepository healthHandlersRepository;
        public DecreaseHealthResult DecreaseHealth(string playerId, int amount)
        {
            var currentHealth = healthHandlersRepository.GetHealth(playerId);
            if (currentHealth <= 0)
                return DecreaseHealthResult.NoHealth;

            currentHealth = Math.Max(0, currentHealth - amount);
            healthHandlersRepository.SetHealth(playerId, currentHealth);
            return currentHealth > 0 ? DecreaseHealthResult.Decreased : DecreaseHealthResult.HealthRanOut;
        }
    }
}
using System;
using Multiplayer.Health.domain.repositories;
using UniRx;
using Zenject;

namespace Multiplayer.Health.data.repositories
{
    public class PlayerHealthDefaultRepository: IPlayerHealthRepository
    {
         private readonly BehaviorSubject<int> healthSubject;

        [Inject]
        public PlayerHealthDefaultRepository(IMaxHealthRepository maxHealthRepository)
        {
            var maxHealth = maxHealthRepository.GetMaxHealth();
            healthSubject = new BehaviorSubject<int>(maxHealth);
        }

        void IPlayerHealthRepository.SetHealth(int health) => healthSubject.OnNext(health);

        public int GetHealth() => healthSubject.Value;

        public IObservable<int> GetHealthFlow() => healthSubject;
    }
}
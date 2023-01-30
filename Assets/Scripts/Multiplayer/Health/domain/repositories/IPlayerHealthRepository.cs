using System;

namespace Multiplayer.Health.domain.repositories
{
    public interface IPlayerHealthRepository
    {
        internal void SetHealth(int health);
        public int GetHealth();
        public IObservable<int> GetHealthFlow();
    }
}
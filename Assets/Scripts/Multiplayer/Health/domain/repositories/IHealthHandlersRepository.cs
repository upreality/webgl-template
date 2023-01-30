using System;

namespace Multiplayer.Health.domain.repositories
{
    public interface IHealthHandlersRepository
    {
        public int GetHealth(string handlerId);
        public void SetHealth(string handlerId, int health);
        public IObservable<int> GetHealthFlow(string handlerId);
    }
}
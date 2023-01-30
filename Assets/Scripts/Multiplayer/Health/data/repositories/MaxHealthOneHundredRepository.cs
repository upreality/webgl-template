using Multiplayer.Health.domain.repositories;

namespace Multiplayer.Health.data.repositories
{
    public class MaxHealthOneHundredRepository: IMaxHealthRepository
    {
        public int GetMaxHealth() => 100;
    }
}
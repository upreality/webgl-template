using System;

namespace Multiplayer.SpawnPoints.domain.repositories
{
    public interface ISpawnPointCooldownRepository
    {
        public IObservable<int> GetCooldownFlow(int pointId);
        public int GetCooldown(int pointId);
        internal void SetCooldown(int pointId, int cooldown);
    }
}
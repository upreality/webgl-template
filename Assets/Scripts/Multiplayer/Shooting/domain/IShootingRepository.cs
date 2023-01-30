using System;
using Multiplayer.Shooting.domain.model;

namespace Multiplayer.Shooting.domain
{
    public interface IShootingRepository
    {
        public IObservable<Shoot> GetShotsFlow(string sourceID);
        public void AddShoot(Shoot shoot);
    }
}
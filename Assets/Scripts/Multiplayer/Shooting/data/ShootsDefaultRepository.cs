using System;
using Multiplayer.Shooting.domain;
using Multiplayer.Shooting.domain.model;
using UniRx;

namespace Multiplayer.Shooting.data
{
    public class ShootsDefaultRepository: IShootingRepository
    {
        private Subject<Shoot> shootsSubject = new();

        public IObservable<Shoot> GetShotsFlow(string sourceID) => shootsSubject
            .Where(shoot => shoot.ShooterID == sourceID);

        public void AddShoot(Shoot shoot) => shootsSubject.OnNext(shoot);
    }
}
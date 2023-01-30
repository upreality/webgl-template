using Multiplayer.MatchTimer.data;
using Multiplayer.MatchTimer.domain;
using Multiplayer.MatchTimer.domain.repositories;
using UnityEngine;
using Zenject;

namespace Multiplayer.MatchTimer._di
{
    [CreateAssetMenu(menuName = "Installers/MatchTimerBaseInstaller")]
    internal class MatchTimerBaseInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            //Data
            Container.Bind<IMatchTimerRepository>()
                .To<MatchTimerDefaultRepository>()
                .AsSingle();
            //Domain
            Container.Bind<TimerCompletedEventUseCase>().ToSelf().AsSingle();
        }
    }
}
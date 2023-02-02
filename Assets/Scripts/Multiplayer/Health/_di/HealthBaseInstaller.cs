using Multiplayer.Health.data.repositories;
using Multiplayer.Health.domain;
using Multiplayer.Health.domain.repositories;
using UnityEngine;
using Zenject;

namespace Multiplayer.Health._di
{
    [CreateAssetMenu(fileName = "HealthBaseInstaller", menuName = "Installers/HealthBaseInstaller", order = 0)]
    public class HealthBaseInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            //Data
            Container.Bind<IMaxHealthRepository>().To<MaxHealthOneHundredRepository>().AsSingle();
            Container.Bind<IPlayerHealthRepository>().To<PlayerHealthDefaultRepository>().AsSingle();
            //Presentation
            Container.Bind<DecreaseHealthUseCase>().ToSelf().AsSingle();
            Container.Bind<IncreaseHealthUseCase>().ToSelf().AsSingle();
            Container.Bind<RestoreHealthUseCase>().ToSelf().AsSingle();
            Container.Bind<RelativeHealthUseCase>().ToSelf().AsSingle();
            Container.Bind<PlayerDeathEventUseCase>().ToSelf().AsSingle();
        }
    }
}

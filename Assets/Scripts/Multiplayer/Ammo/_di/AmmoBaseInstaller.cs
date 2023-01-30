using Multiplayer.Ammo.data;
using Multiplayer.Ammo.domain;
using Multiplayer.Ammo.domain.repository;
using UnityEngine;
using Zenject;

namespace Multiplayer.Ammo._di
{
    [CreateAssetMenu(fileName = "AmmoBaseInstaller",menuName = "Installers/AmmoBaseInstaller")]
    public class AmmoBaseInstaller: ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            //Data
            Container.Bind<IAmmoRepository>().To<AmmoDefaultRepository>().AsSingle();
            Container.Bind<IAmmoStateRepository>().To<AmmoStateDefaultRepository>().AsSingle();
            //Domain
            Container.Bind<AmmoAvailableStateUseCase>().ToSelf().AsSingle();
            Container.Bind<GetReloadingStateUseCase>().ToSelf().AsSingle();
            Container.Bind<GetReloadRequiredStateUseCase>().ToSelf().AsSingle();
            Container.Bind<PassAmmoUseCase>().ToSelf().AsSingle();
            Container.Bind<ReloadAmmoUseCase>().ToSelf().AsSingle();
        }
    }
}
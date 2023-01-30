using Multiplayer.Shooting.data;
using Multiplayer.Shooting.domain;
using UnityEngine;
using Zenject;

namespace Multiplayer.Shooting._di
{
    [CreateAssetMenu(fileName = "ShootingBaseInstaller", menuName = "Installers/ShootingBaseInstaller")]
    public class ShootingBaseInstaller: ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            //Data
            Container.Bind<IShootingRepository>().To<ShootsDefaultRepository>().AsSingle();
        }
    }
}
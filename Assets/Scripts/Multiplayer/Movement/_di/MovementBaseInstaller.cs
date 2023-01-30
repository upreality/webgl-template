using Multiplayer.Movement.data;
using Multiplayer.Movement.domain;
using UnityEngine;
using Zenject;

namespace Multiplayer.Movement._di
{
    [CreateAssetMenu(fileName = "MovementBaseInstaller", menuName = "Installers/MovementBaseInstaller", order = 0)]
    public class MovementBaseInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            //Data
            Container.Bind<IMovementStateRepository>().To<MovementStateDefaultRepository>().AsSingle();
            Container.Bind<IJumpingStateRepository>().To<JumpingStateDefaultRepository>().AsSingle();
            //Domain
            Container.Bind<CurrentPlayerMovementStateUseCase>().ToSelf().AsSingle();
        }
    }
}
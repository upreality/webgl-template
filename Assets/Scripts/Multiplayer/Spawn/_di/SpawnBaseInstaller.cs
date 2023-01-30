using Multiplayer.Spawn.data;
using Multiplayer.Spawn.domain;
using Multiplayer.Spawn.domain.repositories;
using UnityEngine;
using Zenject;

namespace Multiplayer.Spawn._di
{
    [CreateAssetMenu(menuName = "Installers/SpawnBaseInstaller")]
    public class SpawnBaseInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            //Data
            Container
                .Bind<IPlayerSpawnEventRepository>()
                .To<PlayerSpawnEventDefaultRepository>()
                .AsSingle();
            //Domain
            Container.Bind<SpawnPlayerAvailableUseCase>().ToSelf().AsSingle();
            Container.Bind<SpawnCurrentPlayerUseCase>().ToSelf().AsSingle();
            Container.Bind<ReadyAfterDeathUseCase>().ToSelf().AsSingle();
        }
    }
}
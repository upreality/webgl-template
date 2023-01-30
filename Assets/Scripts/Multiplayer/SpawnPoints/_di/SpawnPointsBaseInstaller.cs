using Multiplayer.Spawn.presentation.Spawner;
using Multiplayer.SpawnPoints.data;
using Multiplayer.SpawnPoints.domain;
using Multiplayer.SpawnPoints.domain.repositories;
using Multiplayer.SpawnPoints.presentation;
using UnityEngine;
using Zenject;

namespace Multiplayer.SpawnPoints._di
{
    [CreateAssetMenu(fileName = "SpawnPointsBaseInstaller", menuName = "Installers/ScriptableObjectInstaller")]
    public class SpawnPointsBaseInstaller: ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            //Data
            Container
                .Bind<ISpawnPointsRepository>()
                .To<SpawnPointsDefaultRepository>()
                .AsSingle();
            Container
                .Bind<ISpawnPointCooldownRepository>()
                .To<SpawnPointCooldownDefaultRepository>()
                .AsSingle();
            //Domain
            Container.Bind<SpawnPointAvailableUseCase>().ToSelf().AsSingle();
            Container.Bind<RandomSpawnPointUseCase>().ToSelf().AsSingle();
            //Presentation
            var positionNavigator = FindObjectOfType<SimpleSpawnPoints>();
            Container.Bind<ISpawnPositionNavigator>().FromInstance(positionNavigator).AsSingle();
        }
    }
}
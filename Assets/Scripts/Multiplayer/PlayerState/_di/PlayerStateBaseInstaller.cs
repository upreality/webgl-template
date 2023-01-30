using Multiplayer.PlayerState.data;
using Multiplayer.PlayerState.domain;
using Multiplayer.PlayerState.domain.repositories;
using UnityEngine;
using Zenject;

namespace Multiplayer.PlayerState._di
{
    [CreateAssetMenu(menuName = "Installers/PlayerStateBaseInstaller")]
    public class PlayerStateBaseInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            //Data
            Container
                .Bind<IPlayerLifecycleEventRepository>()
                .To<PlayerLifecycleEventDefaultRepository>()
                .AsSingle();

            Container
                .Bind<IPlayerStateRepository>()
                .To<PlayerStateSelfUpdateRepository>()
                .AsSingle();

            //Domain
            Container.Bind<PlayerStateUpdatesUseCase>().ToSelf().AsSingle();
        }
    }
}
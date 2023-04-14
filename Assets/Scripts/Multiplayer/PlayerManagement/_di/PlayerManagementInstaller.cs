using Multiplayer.PlayerManagement.data;
using Multiplayer.PlayerManagement.presentation;
using UnityEngine;
using Zenject;

namespace Multiplayer.PlayerManagement._di
{
    [CreateAssetMenu(menuName = "Installers/PlayerManagementInstaller")]
    public class PlayerManagementInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            var instance = FindObjectOfType<PlayerIdsRepository>();
            Container.BindInterfacesAndSelfTo<PlayerIdsRepository>().FromInstance(instance).AsSingle();
            
            Container.BindInterfacesAndSelfTo<ConnectionEventsDebug>().AsSingle();
        }
    }
}
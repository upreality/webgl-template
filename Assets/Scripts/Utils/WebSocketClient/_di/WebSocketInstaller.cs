using UnityEngine;
using Utils.WebSocketClient.data;
using Utils.WebSocketClient.domain;
using Zenject;

namespace Utils.WebSocketClient._di
{
    [CreateAssetMenu(menuName = "Installers/WebSocketInstaller")]
    public class WebSocketInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private WSSettings settings;
        public override void InstallBindings()
        {
            Container.Bind<WSSettings>().FromInstance(settings).AsSingle();
            
            var socketStorage = new WebSocketStorage();
            Container.Bind<WebSocketStorage>().FromInstance(socketStorage).AsSingle();
            Container.BindInterfacesAndSelfTo<InMemoryWSCommandsRepository>().AsSingle();

            var wsConnectionRepository = FindObjectOfType<MonoWSConnectionRepository>();
            Container.Bind<IWSConnectionRepository>().FromInstance(wsConnectionRepository).AsSingle();
            
            Container
                .Bind<IWSCommandsUseCase>()
                .WithId(IWSCommandsUseCase.BaseInstance)
                .To<WSCommandsUseCase>()
                .AsSingle();
        }
    }
}
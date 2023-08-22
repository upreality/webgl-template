using HNS.data;
using HNS.domain;
using UnityEngine;
using Zenject;

namespace HNS._di
{
    public class HNSInstaller : MonoInstaller
    {
        [SerializeField] private SleepPlacesSceneRepository sleepPlacesSceneRepository;

        public override void InstallBindings()
        {
            base.InstallBindings();
            Container
                .BindInterfacesAndSelfTo<SleepPlacesSceneRepository>()
                .FromInstance(sleepPlacesSceneRepository)
                .AsSingle();

            
            Container.Bind<HNSGameStateUseCase>().AsSingle();
            Container.BindInterfacesAndSelfTo<CatcherHandsUseCase>().AsSingle();
            Container.BindInterfacesAndSelfTo<HNSPlayerSnapshotsUseCase>().AsSingle();
            Container.BindInterfacesAndSelfTo<HNSPlayerSnapshotsUseCase>().AsSingle();
            Container.BindInterfacesAndSelfTo<HiderStateUseCase>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerHiderStateUseCase>().AsSingle();
        }
    }
}
using HNS.data;
using HNS.domain;
using HNS.Player;
using UnityEngine;
using Zenject;

namespace HNS._di
{
    public class HNSInstaller: MonoInstaller
    {
        [SerializeField] private SleepPlacesSceneRepository sleepPlacesSceneRepository;
        public override void InstallBindings()
        {
            base.InstallBindings();
            Container.Bind<HNSGameStateRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<CatcherHandsRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<HNSSnapshotRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<SleepPlacesSceneRepository>().FromInstance(sleepPlacesSceneRepository).AsSingle();
            
            Container.BindInterfacesAndSelfTo<HNSSnapshotUseCase>().AsSingle();
            Container.BindInterfacesAndSelfTo<HiderStateUseCase>().AsSingle();
        }
    }
}
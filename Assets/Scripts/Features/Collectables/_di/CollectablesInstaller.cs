using Features.Coins.data;
using UnityEngine;
using Zenject;

namespace Features.Coins._di
{
    public class CollectablesInstaller: MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LocalStorageCollectableRepository>().AsSingle();
        }
    }
}
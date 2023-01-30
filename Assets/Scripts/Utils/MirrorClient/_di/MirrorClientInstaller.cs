using UnityEngine;
using Utils.MirrorClient.data;
using Utils.MirrorClient.domain;
using Zenject;

namespace Utils.MirrorClient._di
{
    [CreateAssetMenu(menuName = "Installers/MirrorClientInstaller")]
    public class MirrorClientInstaller: ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MirrorMatchConnectedStateRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<MirrorServerRepository>().AsSingle();
            Container.BindInterfacesAndSelfTo<MirrorCommandUseCase>().AsSingle();
        }
    }
}
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "MirrorRoomParamInstaller", menuName = "Installers/MirrorRoomParamInstaller")]
public class MirrorRoomParamInstaller : ScriptableObjectInstaller
{
    public override void InstallBindings()
    {
        var handler = FindObjectOfType<MirrorRoomParamsHandler>();
        Container.BindInterfacesAndSelfTo<MirrorRoomParamsHandler>().FromInstance(handler).AsSingle();
        var handler2 = FindObjectOfType<MirrorPlayerParamsHandler>();
        Container.BindInterfacesAndSelfTo<MirrorPlayerParamsHandler>().FromInstance(handler2).AsSingle();
    }
}
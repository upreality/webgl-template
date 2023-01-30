using Features.Cameras.data;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Features.Cameras._di
{
    [CreateAssetMenu(menuName = "Installers/CamerasInstaller")]
    public class CamerasInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private CameraTypeSORepository camTypeRepository;

        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<CameraTypeSORepository>()
                .FromInstance(camTypeRepository)
                .AsSingle();

            var activeCamRepository = FindObjectOfType<ActiveCameraMonoRepository>();
            Container
                .BindInterfacesAndSelfTo<ActiveCameraMonoRepository>()
                .FromInstance(activeCamRepository)
                .AsSingle();
        }
    }
}
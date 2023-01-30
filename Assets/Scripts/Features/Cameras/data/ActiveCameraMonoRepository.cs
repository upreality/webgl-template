using System;
using Features.Cameras.domain;
using Features.Cameras.domain.model;
using UniRx;
using UnityEngine;
using Utils.StringSelector;
using Zenject;

namespace Features.Cameras.data
{
    public class ActiveCameraMonoRepository : MonoBehaviour, IActiveCameraRepository
    {
        [Inject] private ICameraTypeRepository cameraTypeRepository;

        [SerializeField, StringSelector(typeof(CameraTypeSORepository))]
        private string defaultCamTypeId;

        private BehaviorSubject<CamType> activeCameraTypeSubject;

        private void Awake()
        {
            var defaultCamType = cameraTypeRepository.Get(defaultCamTypeId);
            activeCameraTypeSubject = new BehaviorSubject<CamType>(defaultCamType);
        }

        public IObservable<CamType> GetActiveCameraFlow() => activeCameraTypeSubject;

        public void SetActiveCamera(CamType type) => activeCameraTypeSubject.OnNext(type);
    }
}
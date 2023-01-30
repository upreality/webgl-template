using Features.Cameras.data;
using Features.Cameras.domain;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using Utils.StringSelector;
using Zenject;

namespace Features.Cameras.presentation
{
    [RequireComponent(typeof(Camera))]
    public class SelectableCamera : MonoBehaviour
    {
        [Inject] private IActiveCameraRepository cameraRepository;

        [SerializeField, StringSelector(typeof(CameraTypeSORepository))]
        private string camTypeId;

        [SerializeField, CanBeNull] private UnityEvent onSelected;

        private Camera target;

        private void Start()
        {
            target = GetComponent<Camera>();
            cameraRepository
                .GetActiveCameraFlow()
                .Select(activeCam => camTypeId == activeCam.Id)
                .DistinctUntilChanged()
                .Subscribe(SetSelectedState)
                .AddTo(this);
        }

        private void SetSelectedState(bool selected)
        {
            target.enabled = selected;
            if (!selected) return;
            onSelected?.Invoke();
        }
    }
}
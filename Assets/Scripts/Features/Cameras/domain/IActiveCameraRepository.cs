using System;
using Features.Cameras.domain.model;

namespace Features.Cameras.domain
{
    public interface IActiveCameraRepository
    {
        IObservable<CamType> GetActiveCameraFlow();
        void SetActiveCamera(string cameraId);
    }
}
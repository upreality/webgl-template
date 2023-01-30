using System;

namespace Multiplayer.Ammo.presentation.navigator
{
    public interface IReloadHandler
    {
        IObservable<ReloadingHandlerResult> StartReloading();
        void AbortReloading();

        public enum ReloadingHandlerResult
        {
            Completed,
            Aborted,
            InvalidState
        }
    }
}
using System;
using JetBrains.Annotations;
using Multiplayer.Ammo.domain;
using Multiplayer.Ammo.domain.model;
using Multiplayer.Ammo.domain.repository;
using UniRx;
using UnityEngine;
using Zenject;

namespace Multiplayer.Ammo.presentation.navigator
{
    public class ReloadNavigator
    {
        [Inject] private ReloadAmmoUseCase reloadAmmoUseCase;
        [Inject] private IAmmoStateRepository ammoStateRepository;
        [Inject] private AmmoAvailableStateUseCase ammoAvailableStateUseCase;

        [CanBeNull] private IReloadHandler reloadHandler;

        public IObservable<ReloadingResult> StartReloading()
        {
            if (reloadHandler == null) return Observable.Return(ReloadingResult.WrongState);

            var currentState = ammoStateRepository.GetAmmoState();
            var reloadableAmmoState = currentState is AmmoState.Empty or AmmoState.Loaded;
            var ammoAvailableState = ammoAvailableStateUseCase.GetAmmoAvailableState();
            if (!reloadableAmmoState || !ammoAvailableState) return Observable.Return(ReloadingResult.WrongState);

            ammoStateRepository.SetAmmoState(AmmoState.Reloading);
            return reloadHandler
                .StartReloading()
                .Select(GetReloadingResult)
                .Do(ApplyReloadingResult);
        }

        public void SetReloadHandler(IReloadHandler handler)
        {
            reloadHandler?.AbortReloading();
            reloadHandler = handler;
        }

        private static ReloadingResult GetReloadingResult(IReloadHandler.ReloadingHandlerResult handlerResult)
        {
            ReloadingResult reloadingResult;
            switch (handlerResult)
            {
                case IReloadHandler.ReloadingHandlerResult.Completed:
                    reloadingResult = ReloadingResult.Success;
                    break;
                case IReloadHandler.ReloadingHandlerResult.Aborted:
                    reloadingResult = ReloadingResult.Stopped;
                    break;
                default:
                    Debug.LogError("IReloadPresenter.ReloadingPresenterResult out of range");
                    reloadingResult = ReloadingResult.Stopped;
                    break;
            }

            return reloadingResult;
        }

        private void ApplyReloadingResult(ReloadingResult result)
        {
            if(result!=ReloadingResult.Success) return;
            reloadAmmoUseCase.ReloadAmmo();
        }

        public enum ReloadingResult
        {
            Success,
            Stopped,
            WrongState
        }
    }
}
using System;
using Multiplayer.Ammo.domain.model;
using Multiplayer.Ammo.domain.repository;
using UniRx;
using Zenject;

namespace Multiplayer.Ammo.domain
{
    public class GetReloadingStateUseCase
    {
        [Inject] private IAmmoStateRepository stateRepository;
        
        public IObservable<bool> GetIsReloadingFlow() => stateRepository.GetAmmoStateFlow().Select(state => state == AmmoState.Reloading);
    }
}
using System;
using Multiplayer.Ammo.domain.model;
using Multiplayer.Ammo.domain.repository;
using UniRx;
using Zenject;

namespace Multiplayer.Ammo.domain
{
    public class GetReloadRequiredStateUseCase
    {
        [Inject] private IAmmoStateRepository stateRepository;
        
        public IObservable<bool> GetReloadRequiredFlow() => stateRepository.GetAmmoStateFlow().Select(state => state == AmmoState.Empty);
    }
}
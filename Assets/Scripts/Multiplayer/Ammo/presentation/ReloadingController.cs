using Multiplayer.Ammo.presentation.navigator;
using Multiplayer.PlayerInput.domain;
using Multiplayer.PlayerInput.domain.model;
using UniRx;
using UnityEngine;
using Zenject;

namespace Multiplayer.Ammo.presentation
{
    public class ReloadingController: MonoBehaviour
    {
        
        [Inject] private ReloadNavigator navigator;
        [Inject] private PlayerInputUseCase inputUseCase;

        private void Update()
        {
            if (!(inputUseCase.GetAxis(PlayerInputAxis.Reload) > 0f)) return;
            
            navigator.StartReloading().Subscribe().AddTo(this);
        }
    }
}
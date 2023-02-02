using System;
using Multiplayer.Health.domain;
using UnityEngine;
using Zenject;

namespace Multiplayer.Health.presentation.DebugComponents
{
    public class DecreasePlayerHPDebug : MonoBehaviour
    {
        [Inject] private DecreaseHealthUseCase decreaseHealthUseCase;
        [SerializeField] private string playerId;
        [SerializeField] private int decreaseAmount = 10;
        [SerializeField] private KeyCode key = KeyCode.H;

        private void Update()
        {
            if(!Input.GetKeyDown(key)) return;
            decreaseHealthUseCase.DecreaseHealth(playerId, 10);
        }
    }
}

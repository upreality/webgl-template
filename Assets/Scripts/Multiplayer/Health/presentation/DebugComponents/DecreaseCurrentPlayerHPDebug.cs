using Multiplayer.Health.domain;
using UnityEngine;
using Zenject;

namespace Multiplayer.Health.presentation.DebugComponents
{
    public class DecreaseCurrentPlayerHPDebug : MonoBehaviour
    {
        [Inject] private DecreaseHealthUseCase decreaseHealthUseCase;
        [SerializeField] private int decreaseAmount = 10;
        [SerializeField] private KeyCode key = KeyCode.H;

        private void Update()
        {
            if(!Input.GetKeyDown(key)) return;
            decreaseHealthUseCase.DecreaseHealth(10);
        }
    }
}

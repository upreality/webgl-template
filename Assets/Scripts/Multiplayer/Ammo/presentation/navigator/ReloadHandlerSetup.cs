using UnityEngine;
using Zenject;

namespace Multiplayer.Ammo.presentation.navigator
{
    public class ReloadHandlerSetup : MonoBehaviour
    {
        [Inject] private ReloadNavigator navigator;
        private IReloadHandler handler;
        private void Awake() => handler = GetComponent<IReloadHandler>();

        private void OnEnable()
        {
            if (handler == null)
                Debug.LogError("Reload handler setup failed - handler not found!");

            navigator.SetReloadHandler(handler);
        }
    }
}
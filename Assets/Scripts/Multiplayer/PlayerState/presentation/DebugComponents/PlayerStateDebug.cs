using Multiplayer.PlayerState.domain.model;
using Multiplayer.PlayerState.domain.repositories;
using UniRx;
using UnityEngine;
using Utils.Editor;
using Zenject;

namespace Multiplayer.PlayerState.presentation.DebugComponents
{
    public class PlayerStateDebug : MonoBehaviour
    {
        [Inject] private IPlayerStateRepository playerStateRepository;
        [SerializeField, ReadOnly] private PlayerStates currentState = PlayerStates.None;
        [SerializeField] private bool enableLogging = true;

        private void Awake() => playerStateRepository
            .GetPlayerStateFlow()
            .Subscribe(LogPlayerState)
            .AddTo(this);

        private void LogPlayerState(PlayerStates state)
        {
            currentState = state;
            if (!enableLogging) return;
            Debug.Log($"New Player State: {state}");
        }
    }
}
using Multiplayer.MatchState.domain.repositories;
using UniRx;
using UnityEngine;
using Zenject;

namespace Multiplayer.MatchState.presentation.DebugComponents
{
    public class MatchStateDebug : MonoBehaviour
    {
        [SerializeField] private bool logState = true;
        [Inject] private IMatchStateRepository matchStateRepository;

        private void Awake() => matchStateRepository
            .GetMatchStateFlow()
            .Where(_ => logState)
            .Subscribe(state =>
                Debug.Log($"New Match State: {state}")
            ).AddTo(this);
    }
}
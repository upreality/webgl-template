using JetBrains.Annotations;
using Multiplayer.MatchState.domain.model;
using Multiplayer.MatchState.domain.repositories;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Multiplayer.MatchState.presentation.UI
{
    public class MatchStateEvents : MonoBehaviour
    {
        [Inject] private IMatchStateRepository matchStateRepository;

        [SerializeField] private MatchStates state;
        [SerializeField] [CanBeNull] private UnityEvent onState;

        private void Start() => matchStateRepository
            .GetMatchStateFlow()
            .Where(currentState => currentState == state)
            .Subscribe(_ => onState?.Invoke())
            .AddTo(this);
    }
}
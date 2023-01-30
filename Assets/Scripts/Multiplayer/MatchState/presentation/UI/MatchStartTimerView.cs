using Multiplayer.MatchState.domain;
using Multiplayer.MatchState.domain.model;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Multiplayer.MatchState.presentation.UI
{
    public class MatchStartTimerView : MonoBehaviour
    {
        [Inject] private MatchStateTimerStateFlowUseCase matchStateTimerStateFlowUseCase;

        [SerializeField, Tooltip("$ sign will be replaced with mm:ss timer")]
        private string template = "$";

        [SerializeField] private Text timerText;
        [SerializeField] private MatchStates observedState;

        private void Start() => matchStateTimerStateFlowUseCase
            .GetTimerStateFlow(observedState)
            .Subscribe(RenderTimerState)
            .AddTo(this);

        private void RenderTimerState(TimerState state)
        {
            timerText.enabled = state.IsActive;
            if (!template.Contains('$'))
            {
                Debug.LogError("Timer text template does not contains '$' to replace!");
                return;
            }

            var minutes = state.TimeLeft / 60;
            var seconds = state.TimeLeft % 60;
            timerText.text = template.Replace("$", $"{minutes}:{seconds}");
        }
    }
}
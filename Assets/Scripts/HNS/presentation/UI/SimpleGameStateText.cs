using HNS.domain;
using HNS.domain.model;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace HNS.presentation.UI
{
    public class SimpleGameStateText : MonoBehaviour
    {
        [Inject] private HNSGameStateUseCase gameStateUseCase;

        [SerializeField] private TMP_Text text;

        private void Start() => gameStateUseCase
            .GetStateFlow()
            .Subscribe(HandleGameState)
            .AddTo(this);

        private void HandleGameState(GameStates state)
        {
            text.text = state.ToString();
        }
    }
}
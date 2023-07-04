using HNS.Model;
using TMPro;
using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace HNS
{
    public class SimpleGameStateText : MonoBehaviour
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        [SerializeField] private TMP_Text text;

        private void Start() => commandsUseCase
            .Subscribe<long>(Commands.GameState)
            .DistinctUntilChanged()
            .Subscribe(HandleGameState)
            .AddTo(this);

        private void HandleGameState(long stateCode)
        {
            text.text = ((GameStates)stateCode).ToString();
        }
    }
}
using HNS.Model;
using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace HNS
{
    public class SimpleRulette : MonoBehaviour
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        [SerializeField] private GameObject roulette;

        private void Start() => commandsUseCase
            .Subscribe<long>(Commands.GameState)
            .DistinctUntilChanged()
            .Select(code => (GameStates)code)
            .Subscribe(HandleGameState)
            .AddTo(this);

        private void HandleGameState(GameStates state)
        {
            roulette.SetActive(state is GameStates.SettingRoles);
        }
    }
}
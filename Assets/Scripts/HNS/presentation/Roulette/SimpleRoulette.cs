using HNS.domain;
using HNS.domain.Model;
using UniRx;
using UnityEngine;
using Zenject;

namespace HNS.presentation.Roulette
{
    public class SimpleRoulette : MonoBehaviour
    {
        [Inject]
        private HNSGameStateUseCase gameStateUseCase;

        [SerializeField] private GameObject roulette;

        private void Start() => gameStateUseCase
            .GetStateFlow()
            .Subscribe(HandleGameState)
            .AddTo(this);

        private void HandleGameState(GameStates state)
        {
            roulette.SetActive(state is GameStates.SettingRoles);
        }
    }
}
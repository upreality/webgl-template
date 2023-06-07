using System;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace WSExample.TicTac.presentation
{
    public class SimpleGameController : MonoBehaviour
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        [SerializeField] private TMP_Text opponentNickText;

        [Header("Finished")] [SerializeField] private GameObject finishedScreen;
        [SerializeField] private GameObject looseMark;
        [SerializeField] private GameObject winMark;
        [SerializeField] private TMP_Text earned;

        [SerializeField] private GameObject opponent;
        [SerializeField] private GameObject me;

        private CompositeDisposable enabledDisposable = new();

        private void OnEnable()
        {
            finishedScreen.SetActive(false);
            enabledDisposable = new CompositeDisposable();

            commandsUseCase
                .Listen<GameState>(Commands.FightGameUpdate)
                .Subscribe(HandleGameState)
                .AddTo(enabledDisposable);
            commandsUseCase
                .Listen<FinishedData>(Commands.FightFinished)
                .Subscribe(HandleFinish)
                .AddTo(enabledDisposable);
        }

        private void OnDisable() => enabledDisposable.Dispose();

        private void HandleGameState(GameState state)
        {
            finishedScreen.SetActive(state.state is FightGameState.HasWinner or FightGameState.Draw);
            winMark.SetActive(false);
            earned.gameObject.SetActive(false);

            me.transform.position = Vector3.right * state.playerState.pos;
            opponent.transform.position = Vector3.right * state.opponentState.pos;

            looseMark.SetActive(false);
        }

        private void HandleFinish(FinishedData data)
        {
            finishedScreen.SetActive(data.finished);
            winMark.SetActive(data.isWinner);
            earned.gameObject.SetActive(data.isWinner);
            earned.text = $"Earned {data.reward} coins!";
            looseMark.SetActive(!data.isWinner);
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [Serializable]
        public struct GameState
        {
            public PlayerState playerState;
            public PlayerState opponentState;
            public int stateCode;

            public FightGameState state => (FightGameState)stateCode;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        [Serializable]
        public struct PlayerState
        {
            public float pos;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private struct FinishedData
        {
            public bool finished;
            public bool isWinner;
            public bool isDraw;
            public long reward;
        }

        public enum FightGameState
        {
            Undefined = 0,
            Preparing = 1,
            Playing = 2,
            HasWinner = 3,
            Draw = 4,
        }
    }
}
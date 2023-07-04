using System;
using Core.Auth.domain;
using HNS.domain;
using HNS.domain.Model;
using SUPERCharacter;
using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace HNS.presentation.Player.Hider
{
    public class HiderControllerPlayer : MonoBehaviour
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;
        [Inject] private HNSPlayerSnapshotsUseCase snapshots;
        [Inject] private IAuthRepository authRepository;

        [SerializeField]
        private SUPERCharacterAIO controller;
        [SerializeField]
        private PlayerMovementSender sender;
        [SerializeField]
        private Animator animator;

        public float speed = 5f;

        private BehaviorSubject<bool> isMovableFlow;

        private IObservable<GameStates> GetGameStatesFlow() => commandsUseCase
            .Subscribe<long>(Commands.GameState)
            .DistinctUntilChanged()
            .Select(state => (GameStates)state);
        
        private IObservable<HiderSnapshotItem> GetSnapshotFlow(long playerId) => snapshots
            .GetHider(playerId, out var flow)
            ? flow
            : Observable.Empty<HiderSnapshotItem>();

        private void Start()
        {
            var playerId = Convert.ToInt64(authRepository.LoginUserId);
            var snapshotFlow = GetSnapshotFlow(playerId);
            GetGameStatesFlow()
                .CombineLatest(snapshotFlow, (state, snapshot) => new Tuple<GameStates, HiderSnapshotItem>(state, snapshot))
                .Subscribe(stateToSnapshot => OnStateChanged(stateToSnapshot.Item1, stateToSnapshot.Item2))
                .AddTo(this);
            // GetSnapshotFlow(playerId).Subscribe()
        }

        private void OnStateChanged(GameStates state, HiderSnapshotItem item)
        {
            var isPlaying = state == GameStates.Searching;
            controller.enableMovementControl = isPlaying;
            
            if (isPlaying) sender.StartSending();
            else sender.StopSending();
        }
        
        private struct PlayerData
        {
            public HiderSnapshotItem Itemtem;
            public GameStates State;
        }
    }
}
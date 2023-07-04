using System;
using HNS.domain;
using HNS.Model;
using UniRx;
using UnityEngine;
using Utils.WebSocketClient.domain;
using Utils.WebSocketClient.domain.model;
using Zenject;

namespace HNS.Player
{
    public class PlayerMovementSender : MonoBehaviour
    {
        [Inject(Id = IWSCommandsUseCase.AuthorizedInstance)]
        private IWSCommandsUseCase commandsUseCase;

        private readonly CompositeDisposable sendMovementHandler = new();

        [SerializeField] private int syncFreqMs = 100;
        [SerializeField] private bool startSendingOnStart;

        private TimeSpan SyncPeriod => TimeSpan.FromMilliseconds(syncFreqMs);

        private void Start()
        {
            if (!startSendingOnStart) 
                return;
            
            StartSending();
        }

        public void StartSending()
        {
            sendMovementHandler.Clear();
            Observable
                .Timer(SyncPeriod)
                .Repeat()
                .StartWith(0L)
                .Select(_ => transform.GetSnapshot())
                .Subscribe(SendSnapshot)
                .AddTo(sendMovementHandler);
        }

        public void StopSending() => sendMovementHandler.Clear();

        private void SendSnapshot(TransformSnapshot snapshot) => commandsUseCase
            .Request<long, TransformSnapshot>(Commands.Movement, snapshot)
            .Subscribe()
            .AddTo(sendMovementHandler);

        private void OnDestroy() => sendMovementHandler.Clear();
    }
}
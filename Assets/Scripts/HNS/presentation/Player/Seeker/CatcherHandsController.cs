using System;
using HNS.domain;
using UniRx;
using UnityEngine;
using Zenject;

namespace HNS.presentation.Player.Seeker
{
    public class CatcherHandsController : MonoBehaviour
    {
        [Inject] private CatcherHandsUseCase catcherHandsUseCase;

        [SerializeField] private PlayerIdProvider idProvider;

        [SerializeField] private Transform handsTransform;

        private long? catcherId;

        private IObservable<long> PlayerIdFlow => idProvider
            .PlayerIdFlow
            .Do(id => catcherId = id);

        private void Awake()
        {
            if (handsTransform != null)
                return;

            handsTransform = transform;
        }

        private void Start() => Observable
            .EveryUpdate()
            .WithLatestFrom(PlayerIdFlow, (_, playerId) => playerId)
            .Subscribe(playerId => catcherHandsUseCase.Set(playerId, handsTransform.position))
            .AddTo(this);

        private void OnDestroy()
        {
            if (!catcherId.HasValue) 
                return;
            
            catcherHandsUseCase.Remove(catcherId.Value);
        }
    }
}
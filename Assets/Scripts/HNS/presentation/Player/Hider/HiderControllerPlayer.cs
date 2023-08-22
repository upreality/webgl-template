using HNS.domain;
using HNS.domain.model;
using SUPERCharacter;
using UniRx;
using UnityEngine;
using Zenject;

namespace HNS.presentation.Player.Hider
{
    public class HiderControllerPlayer : MonoBehaviour
    {
        [Inject] private PlayerHiderStateUseCase stateUseCase;

        [SerializeField] private SUPERCharacterAIO controller;
        [SerializeField] private PlayerMovementSender sender;
        [SerializeField] private HiderAnimationStateController animationController;

        private BehaviorSubject<bool> isMovableFlow;

        private void Start() => stateUseCase
            .GetStateFlow()
            .Subscribe(OnStateChanged)
            .AddTo(this);

        private void OnStateChanged(HiderState item)
        {
            var canMove = item.Behavior == HiderBehavior.Hiding;
            controller.enableMovementControl = canMove;
            if (canMove)
            {
                sender.StartSending();
            }
            else sender.StopSending();
        }
    }
}
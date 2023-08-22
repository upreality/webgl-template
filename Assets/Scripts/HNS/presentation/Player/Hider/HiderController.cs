using System;
using HNS.domain;
using HNS.domain.model;
using UniRx;
using UnityEngine;
using Zenject;
using static HNS.presentation.Player.Hider.HiderAnimationStateController;

namespace HNS.presentation.Player.Hider
{
    public class HiderController : MonoBehaviour
    {
        [Inject] private HiderStateUseCase stateUseCase;
        [Inject] private PlayerViewPrefabProvider playerViewPrefabProvider;

        [SerializeField] private PlayerIdProvider idProvider;

        private HiderAnimationStateController animationController;
        private HNSCharacterController characterController;

        private bool initialized;

        private void Initialize(HiderState state)
        {
            if (initialized)
                return;

            var view = playerViewPrefabProvider.GetCharacter(state.Character);
            var spawnedCharacter = Instantiate(view, transform);
            var spawnedTransform = spawnedCharacter.transform;
            spawnedTransform.localPosition = Vector3.zero;
            animationController = spawnedTransform.GetComponent<HiderAnimationStateController>();
            transform.ApplySnapshot(state.Transform);
            initialized = true;
        }

        private IObservable<Unit> HandleCatchedState(TransformSnapshot handsPos) => characterController
            .FlyTo(handsPos)
            .Select(state => state == MovementState.Moving ? HiderAnimationState.Flying : HiderAnimationState.Catched)
            .Do(animationController.SetAnimationState)
            .AsUnitObservable();

        private IObservable<Unit> HandleLayedState(TransformSnapshot sleepPos) => characterController
            .FlyTo(sleepPos)
            .Select(state => state == MovementState.Moving ? HiderAnimationState.Flying : HiderAnimationState.Sleeping)
            .Do(animationController.SetAnimationState)
            .AsUnitObservable();

        private IObservable<Unit> HandleHidingState(TransformSnapshot targetPos) => characterController
            .RunTo(targetPos)
            .Select(_ => characterController.GetHidingAnimationState())
            .Do(animationController.SetAnimationState)
            .AsUnitObservable();

        private IObservable<Unit> HandlePendingState() => Observable
            .EveryUpdate()
            .Select(_ => characterController.GetIdleAnimationState())
            .Do(animationController.SetAnimationState)
            .AsUnitObservable();

        private IObservable<Unit> HandleState(HiderState state)
        {
            characterController.MovementAllowed = state.Behavior != HiderBehavior.Pending;
            return state.Behavior switch
            {
                HiderBehavior.Pending => HandlePendingState(),
                HiderBehavior.Hiding => HandleHidingState(state.Transform),
                HiderBehavior.Catched => HandleCatchedState(state.Transform),
                HiderBehavior.Layed => HandleLayedState(state.Transform),
                _ => Observable.Return(Unit.Default)
            };
        }

        private void Start() => idProvider
            .PlayerIdFlow
            .Select(stateUseCase.GetStateFlow)
            .Switch()
            .Do(Initialize)
            .DoOnCompleted(Destroy)
            .Select(HandleState)
            .Switch()
            .Subscribe()
            .AddTo(gameObject);

        private void Destroy()
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }

    internal static class CharacterControllerExtensions
    {
        private static HiderAnimationState GetGroundedHidingAnimationState(
            this HNSCharacterController controller
        ) => controller.Velocity > 0.1f
            ? HiderAnimationState.Running
            : HiderAnimationState.Idle;

        internal static HiderAnimationState GetHidingAnimationState(
            this HNSCharacterController controller
        ) => controller.Grounded
            ? controller.GetGroundedHidingAnimationState()
            : HiderAnimationState.Falling;
        
        internal static HiderAnimationState GetIdleAnimationState(
            this HNSCharacterController controller
        ) => controller.Grounded
            ? HiderAnimationState.Idle
            : HiderAnimationState.Falling;
    }
}
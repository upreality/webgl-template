using System;
using HNS.domain;
using HNS.Model;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using static HNS.Player.Hider.HiderAnimationController;

namespace HNS.Player.Hider
{
    public class HiderController : MonoBehaviour
    {
        [Inject] private HiderStateUseCase stateUseCase;
        [Inject] private PlayerViewPrefabProvider playerViewPrefabProvider;

        [SerializeField] private PlayerIdProvider idProvider;

        [SerializeField] private Rigidbody rb;
        [SerializeField] private NavMeshAgent agent;

        private Transform controllerRoot;
        private HiderAnimationController animationController;

        private HiderAnimationState IdleAnimationState => agent.isOnNavMesh
            ? HiderAnimationState.Idle
            : HiderAnimationState.Falling;

        private HiderAnimationState GroundedHidingAnimationState => rb.velocity.magnitude > 0.1f
            ? HiderAnimationState.Running
            : HiderAnimationState.Idle;

        private HiderAnimationState HidingAnimationState => agent.isOnNavMesh
            ? GroundedHidingAnimationState
            : HiderAnimationState.Falling;

        private bool initialized;

        private void Initialize(HiderState state)
        {
            if (initialized)
                return;

            var view = playerViewPrefabProvider.GetCharacter(state.Character);
            var spawnedCharacter = Instantiate(view, controllerRoot);
            var spawnedTransform = spawnedCharacter.transform;
            spawnedTransform.localPosition = Vector3.zero;
            animationController = spawnedTransform.GetComponent<HiderAnimationController>();
            controllerRoot.ApplySnapshot(state.Transform);
            initialized = true;
        }

        private void MoveTo(TransformSnapshot pos)
        {
            var targetRotation = Quaternion.Euler(0, pos.r, 0);
            controllerRoot.rotation = Quaternion.Slerp(controllerRoot.rotation, targetRotation, Time.deltaTime);
            controllerRoot.position = Vector3.Slerp(controllerRoot.position, pos.Pos, Time.deltaTime);
        }

        private IObservable<Unit> HandleCatchedState(TransformSnapshot handsPos) => Observable
            .EveryUpdate()
            .Do(_ => MoveTo(handsPos))
            .Select(_ => (controllerRoot.position - handsPos.Pos).sqrMagnitude > 0.4f)
            .Select(isOnWay => isOnWay ? HiderAnimationState.Flying : HiderAnimationState.Catched)
            .Do(animationController.SetAnimationState)
            .AsUnitObservable();

        private IObservable<Unit> HandleLayedState(TransformSnapshot sleepPos) => Observable
            .EveryUpdate()
            .Do(_ => MoveTo(sleepPos))
            .Select(_ => (controllerRoot.position - sleepPos.Pos).sqrMagnitude > 0.4f)
            .Select(isOnWay => isOnWay ? HiderAnimationState.Flying : HiderAnimationState.Sleeping)
            .Do(animationController.SetAnimationState)
            .AsUnitObservable();

        private IObservable<Unit> HandleHidingState(TransformSnapshot targetPos)
        {
            agent.destination = targetPos.Pos;
            return Observable
                .EveryUpdate()
                .Select(_ => HidingAnimationState)
                .Do(animationController.SetAnimationState)
                .AsUnitObservable();
        }

        private IObservable<Unit> HandlePendingState() => Observable
            .EveryUpdate()
            .Select(_ => IdleAnimationState)
            .Do(animationController.SetAnimationState)
            .AsUnitObservable();

        private IObservable<Unit> HandleState(HiderState state)
        {
            agent.enabled = state.Behavior is HiderBehavior.Hiding or HiderBehavior.Pending;
            return state.Behavior switch
            {
                HiderBehavior.Pending => HandlePendingState(),
                HiderBehavior.Hiding => HandleHidingState(state.Transform),
                HiderBehavior.Catched => HandleCatchedState(state.Transform),
                HiderBehavior.Layed => HandleLayedState(state.Transform),
                _ => Observable.Return(Unit.Default)
            };
        }


        private void Awake() => controllerRoot = transform;

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

        private void Update() => rb.isKinematic = agent.isOnNavMesh;

        private void Destroy()
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }
}
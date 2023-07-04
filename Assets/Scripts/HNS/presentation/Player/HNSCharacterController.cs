using System;
using HNS.domain.Model;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using static HNS.presentation.Player.Hider.HiderAnimationController;

namespace HNS.presentation.Player
{
    public class HNSCharacterController : MonoBehaviour
    {
        [SerializeField] private bool movementEnabled = true;
        [SerializeField] private bool physicsEnabled;
        
        [SerializeField] private Rigidbody rb;
        [SerializeField] private NavMeshAgent agent;

        [Header("Settings")]
        [SerializeField] private float destinationReachedOffset = 0.1f;
        [SerializeField] private float flyRotationSpeed = 1f;
        [SerializeField] private float flySpeed = 1f;

        private float sqrDestinationReachedOffset;

        private Transform viewRoot;

        private IDisposable movementDisposable = Disposable.Empty;

        public bool MovementAllowed
        {
            set
            {
                agent.updatePosition = value;
                movementEnabled = value;
            }
        }

        private bool PhysicsEnabled
        {
            set
            {
                physicsEnabled = value;
                agent.enabled = value;
                UpdateRigidbody();
            }
        }

        private void Awake()
        {
            sqrDestinationReachedOffset = destinationReachedOffset * destinationReachedOffset;
            viewRoot = transform;
        }

        private void Update() => UpdateRigidbody();
        
        private void UpdateRigidbody() => rb.isKinematic = agent.isOnNavMesh && physicsEnabled;

        private HiderAnimationState GroundedHidingAnimationState => rb.velocity.magnitude > 0.1f
            ? HiderAnimationState.Running
            : HiderAnimationState.Idle;

        public HiderAnimationState HidingAnimationState => agent.isOnNavMesh
            ? GroundedHidingAnimationState
            : HiderAnimationState.Falling;

        public HiderAnimationState IdleAnimationState => agent.isOnNavMesh
            ? HiderAnimationState.Idle
            : HiderAnimationState.Falling;

        private void DoFlyTo(TransformSnapshot pos)
        {
            var targetRot = Quaternion.Euler(0, pos.r, 0);
            viewRoot.rotation = Quaternion.Slerp(viewRoot.rotation, targetRot, Time.deltaTime * flyRotationSpeed);
            viewRoot.position = Vector3.Slerp(viewRoot.position, pos.Pos, Time.deltaTime * flySpeed);
        }

        private MovementState GetMovementState(Vector3 position)
        {
            if (!movementEnabled)
                return MovementState.MovementDisabled;

            var toPosition = position - viewRoot.position;
            return toPosition.sqrMagnitude > sqrDestinationReachedOffset
                ? MovementState.Moving
                : MovementState.Reached;
        }

        private IDisposable FlyTo(IObserver<MovementState> observer, TransformSnapshot pos)
        {
            var initialState = GetMovementState(pos.Pos);
            observer.OnNext(initialState);

            if (initialState != MovementState.Moving)
            {
                observer.OnCompleted();
                return Disposable.Empty;
            }

            PhysicsEnabled = false;
            movementDisposable.Dispose();
            movementDisposable = Observable
                .EveryUpdate()
                .Do(_ => DoFlyTo(pos))
                .Select(_ => GetMovementState(pos.Pos))
                .Where(state => state != MovementState.Moving)
                .Do(observer.OnNext)
                .First()
                .DoOnCancel(() => PhysicsEnabled = true)
                .DoOnCompleted(() => PhysicsEnabled = true)
                .Subscribe(_ => observer.OnCompleted());
            return movementDisposable;
        }
        
        private IDisposable RunTo(IObserver<MovementState> observer, TransformSnapshot pos)
        {
            var initialState = GetMovementState(pos.Pos);
            observer.OnNext(initialState);

            if (initialState != MovementState.Moving)
            {
                observer.OnCompleted();
                return Disposable.Empty;
            }

            PhysicsEnabled = true;
            agent.destination = pos.Pos;
            movementDisposable.Dispose();
            movementDisposable = Observable
                .EveryUpdate()
                .Select(_ => GetMovementState(pos.Pos))
                .Where(state => state != MovementState.Moving)
                .Do(observer.OnNext)
                .First()
                .Subscribe(_ => observer.OnCompleted());
            return movementDisposable;
        }

        public IObservable<MovementState> FlyTo(TransformSnapshot pos)
        {
            IDisposable Subscribe(IObserver<MovementState> observer) => FlyTo(observer, pos);
            return Observable.Create((Func<IObserver<MovementState>, IDisposable>)Subscribe);
        }
        
        public IObservable<MovementState> RunTo(TransformSnapshot pos)
        {
            IDisposable Subscribe(IObserver<MovementState> observer) => RunTo(observer, pos);
            return Observable.Create((Func<IObserver<MovementState>, IDisposable>)Subscribe);
        }

        public enum MovementState
        {
            Moving,
            Reached,
            MovementDisabled
        }
    }
}
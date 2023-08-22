using System;
using HNS.domain.model;
using UniRx;
using UnityEngine;

namespace HNS.presentation.Player.Hider
{
    public class HNSPlayerCharacterController : MonoBehaviour
    {
        [SerializeField] private bool movementAllowed = true;

        [SerializeField] private Rigidbody rb;

        [Header("Settings")] 
        [SerializeField] private float destinationReachedOffset = 0.1f;
        [SerializeField] private float flyRotationSpeed = 1f;
        [SerializeField] private float flySpeed = 1f;

        private float sqrDestinationReachedOffset;

        private Transform viewRoot;

        private IDisposable movementDisposable = Disposable.Empty;

        private void Awake()
        {
            sqrDestinationReachedOffset = destinationReachedOffset * destinationReachedOffset;
            viewRoot = transform;
        }

        private void DoFlyTo(TransformSnapshot pos)
        {
            var targetRot = Quaternion.Euler(0, pos.r, 0);
            viewRoot.rotation = Quaternion.Slerp(viewRoot.rotation, targetRot, Time.deltaTime * flyRotationSpeed);
            viewRoot.position = Vector3.Slerp(viewRoot.position, pos.Pos, Time.deltaTime * flySpeed);
        }

        private MovementState GetMovementState(Vector3 position)
        {
            if (!movementAllowed)
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

            if (initialState == MovementState.Reached)
            {
                var targetRot = Quaternion.Euler(0, pos.r, 0);
                viewRoot.rotation = targetRot;
                viewRoot.position = pos.Pos;
            }

            if (initialState != MovementState.Moving)
            {
                observer.OnCompleted();
                return Disposable.Empty;
            }

            rb.isKinematic = false;
            movementDisposable.Dispose();
            movementDisposable = Observable
                .EveryUpdate()
                .Do(_ => DoFlyTo(pos))
                .Select(_ => GetMovementState(pos.Pos))
                .Where(state => state != MovementState.Moving)
                .Do(observer.OnNext)
                .First()
                .DoOnCancel(() => rb.isKinematic = true)
                .DoOnCompleted(() => rb.isKinematic = true)
                .Subscribe(_ => observer.OnCompleted());
            return movementDisposable;
        }

        public IObservable<MovementState> FlyTo(TransformSnapshot pos)
        {
            IDisposable Subscribe(IObserver<MovementState> observer) => FlyTo(observer, pos);
            return Observable.Create((Func<IObserver<MovementState>, IDisposable>)Subscribe);
        }
    }
}
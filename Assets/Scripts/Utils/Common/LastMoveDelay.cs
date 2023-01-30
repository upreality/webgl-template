using System;
using UniRx;
using UnityEngine;
using Utils.Editor;

namespace Utils.Misc
{
    public class LastMoveDelay : MonoBehaviour
    {
        [SerializeField, ReadOnly] private float delay;
        [SerializeField, ReadOnly] private float distance;

        private readonly BehaviorSubject<bool> movingState = new(false);

        private Vector3 startMovePos;
        private DateTime startMovingTime;

        private Vector3 prevPos;

        private void Start()
        {
            startMovePos = transform.position;
            prevPos = startMovePos;
            movingState
                .DistinctUntilChanged()
                .Subscribe(OnMovingStateChanged)
                .AddTo(this);
        }

        private void Update()
        {
            var position = transform.position;
            movingState.OnNext(position != prevPos);
            prevPos = position;
        }

        private void OnMovingStateChanged(bool moving)
        {
            if (moving)
            {
                startMovePos = transform.position;
                startMovingTime = DateTime.Now;
                return;
            }

            delay = (float)(DateTime.Now - startMovingTime).TotalMilliseconds;
            distance = (transform.position - startMovePos).magnitude;
        }
    }
}
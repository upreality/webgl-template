using System;
using UniRx;
using UnityEngine;

public class LastMoveDelay : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private float distance;

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
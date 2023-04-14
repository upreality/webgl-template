using System;
using UniRx;
using UnityEngine;

public class WeaponJitter : MonoBehaviour
{
    private Transform weapon;
    private Vector3 defaultWeaponPos;
    private Vector3 prevPlayerPos;

    [SerializeField] private Transform player;
    [Header("Speed")]
    [SerializeField] private float speed;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private float speedUpdateFreq = 0.1f;
    [SerializeField] private float smooth = 1f;
    [Header("Jitter")]
    [SerializeField] private AnimationCurve jitterBySpeed;
    [Header("Walk")] [SerializeField] private float walkStepDuration = 0.5f;
    [SerializeField] private AnimationCurve walkY;
    [Header("Run")] [SerializeField] private float runStepDuration = 0.3f;
    [SerializeField] private float runStart = 1f;
    [SerializeField] private float runFull = 2f;
    [SerializeField] private AnimationCurve runY;

    private IDisposable updateSpeedDisposable;

    private float timer = 0f;
    
    [Header("Debug")]
    [SerializeField] private float jitterForceDebug = 1f;

    private void OnEnable()
    {
        weapon = transform;
        defaultWeaponPos = weapon.localPosition;
        prevPlayerPos = player.position;

        var updateSpeedPeriod = TimeSpan.FromSeconds(speedUpdateFreq);
        updateSpeedDisposable = Observable.Interval(updateSpeedPeriod).Subscribe(_ => RecalculateSpeed());
    }

    private void RecalculateSpeed()
    {
        var currentPlayerPos = player.position;
        var playerMove = currentPlayerPos - prevPlayerPos;
        prevPlayerPos = currentPlayerPos;
        speed = playerMove.magnitude / speedUpdateFreq;
    }

    private void OnDisable() => updateSpeedDisposable?.Dispose();

    private void Update()
    {
        timer += Time.deltaTime;
        smoothSpeed = Mathf.SmoothStep(smoothSpeed, speed, Time.deltaTime * smooth);
        
        var jitterForce = jitterBySpeed.Evaluate(smoothSpeed);
        var walkJitterPos = walkY.Evaluate(timer % walkStepDuration / walkStepDuration);
        var runJitterPos = walkY.Evaluate(timer % runStepDuration / runStepDuration);
        var walkRunLerpAmount = Mathf.Clamp((smoothSpeed - runStart) / (runFull - runStart), 0f, 1f);
        var jitterPos = Mathf.Lerp(walkJitterPos, runJitterPos, walkRunLerpAmount);
        var resultingPos = Mathf.Lerp(defaultWeaponPos.y, defaultWeaponPos.y + jitterPos, jitterForce);
        weapon.localPosition = new Vector3(defaultWeaponPos.x, resultingPos, defaultWeaponPos.z);

        jitterForceDebug = jitterForce;
    }
}
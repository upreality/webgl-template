using System;
using UniRx;
using UnityEngine;

namespace z_MultiplayerVanilla.Scripts
{
    public class WeaponRunRotator : MonoBehaviour
    {
        [SerializeField] private Transform player;

        private Transform weapon;
        private Vector3 defaultWeaponPos;
        private Vector3 defaultWeaponRot;
        private Vector3 prevPlayerPos;
        private Vector3 prevRot;

        [Header("Run")] [SerializeField] private Vector3 runPos;
        [SerializeField] private Vector3 runRot;
        [SerializeField] private float runLimit = 5f;
        [SerializeField] private float runSmooth = 20f;

        [Header("Speed")] [SerializeField] private float speed;
        [SerializeField] private float smoothSpeed;
        [SerializeField] private float speedUpdateFreq = 0.1f;
        [SerializeField] private float smooth = 1f;

        private IDisposable updateSpeedDisposable;

        private void OnEnable()
        {
            weapon = transform;
            defaultWeaponPos = weapon.localPosition;
            defaultWeaponRot = weapon.localEulerAngles;
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
            smoothSpeed = Mathf.SmoothStep(smoothSpeed, speed, Time.deltaTime * smooth);
            var targetPos = smoothSpeed > runLimit ? runPos : defaultWeaponPos;
            var targetRotation = smoothSpeed > runLimit ? runRot : defaultWeaponRot;
            var currentPos = weapon.localPosition;
            var frameSmooth = Time.deltaTime * runSmooth;
            weapon.localPosition = Vector3.Lerp(currentPos, targetPos, frameSmooth);
            prevRot = Vector3.Slerp(prevRot, targetRotation, frameSmooth);
            weapon.localEulerAngles = prevRot;
        }
    }
}
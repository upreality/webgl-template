using System;
using System.Collections.Generic;
using Multiplayer.PlayerInput.domain;
using Multiplayer.PlayerInput.domain.model;
using UnityEngine;
using Zenject;

namespace Multiplayer.Movement.presentation
{
    public class FirstPersonMovement : MonoBehaviour
    {
        [Inject] private PlayerInputUseCase inputUseCase;

        public float speed = 5;
        private Vector2 velocity;

        [Header("Running")] public bool canRun = true;
        public bool IsRunning { get; private set; }
        public float runSpeed = 9;

        /// <summary> Functions to override movement speed. Will use the last added override. </summary>
        public readonly List<Func<float>> SpeedOverrides = new();

        private void FixedUpdate()
        {
            var movement = new Vector2(
                inputUseCase.GetAxis(PlayerInputAxis.HorizontalMovement),
                inputUseCase.GetAxis(PlayerInputAxis.VerticalMovement)
            );
            var running = inputUseCase.GetAxis(PlayerInputAxis.Running) > 0f;
            UpdateMovement(movement, running);
        }

        private void UpdateMovement(Vector2 movement, bool isRunning)
        {
            // Move.
            IsRunning = canRun && isRunning;
            var movingSpeed = IsRunning ? runSpeed : speed;
            if (SpeedOverrides.Count > 0)
                movingSpeed = SpeedOverrides[^1]();
            velocity.x = movement.x * movingSpeed * Time.deltaTime;
            velocity.y = movement.y * movingSpeed * Time.deltaTime;
            transform.Translate(velocity.x, 0, velocity.y);
        }
    }
}
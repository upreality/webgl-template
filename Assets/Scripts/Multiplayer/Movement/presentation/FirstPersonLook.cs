using Multiplayer.PlayerInput.domain;
using Multiplayer.PlayerInput.domain.model;
using UnityEngine;
using Zenject;

namespace Multiplayer.Movement.presentation
{
    public class FirstPersonLook : MonoBehaviour
    {
        [Inject] private PlayerInputUseCase inputUseCase;

        [SerializeField] Transform character;
        private Vector2 currentMouseLook;
        private Vector2 appliedMouseDelta;
        public float sensitivity = 1;
        public float smoothing = 2;

        private void Reset() => character = GetComponentInParent<FirstPersonMovement>().transform;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            var movementDelta = new Vector2(
                inputUseCase.GetAxis(PlayerInputAxis.HorizontalLook),
                inputUseCase.GetAxis(PlayerInputAxis.VerticalLook)
            );
            UpdateLook(movementDelta);
        }

        private void UpdateLook(Vector2 input)
        {
            // Get smooth mouse look.
            var smoothMouseDelta = Vector2.Scale(input, Vector2.one * sensitivity * smoothing);
            appliedMouseDelta = Vector2.Lerp(appliedMouseDelta, smoothMouseDelta, 1 / smoothing);
            currentMouseLook += appliedMouseDelta;
            currentMouseLook.y = Mathf.Clamp(currentMouseLook.y, -90, 90);

            // Rotate camera and controller.
            transform.localRotation = Quaternion.AngleAxis(-currentMouseLook.y, Vector3.right);
            character.localRotation = Quaternion.AngleAxis(currentMouseLook.x, Vector3.up);
        }
    }
}
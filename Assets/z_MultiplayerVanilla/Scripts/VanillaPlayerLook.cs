using Mirror;
using UnityEngine;

namespace z_MultiplayerVanilla.Scripts
{
    public class VanillaPlayerLook : NetworkBehaviour
    {
        [SerializeField] private Transform character;
        [SerializeField] private GameObject cam;

        public float sensitivity = 2;
        public float smoothing = 1.5f;

        private Transform camTransform;

        private Vector2 velocity;
        private Vector2 frameVelocity;

        private void Start()
        {
            camTransform = cam.transform;
            cam.SetActive(isLocalPlayer);
            if (!isLocalPlayer) return;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDestroy()
        {
            cam.SetActive(false);
            if (!isLocalPlayer) return;
            Cursor.lockState = CursorLockMode.None;
        }

        private void Update()
        {
            if (!isLocalPlayer) return;
            // Get smooth velocity.
            Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
            frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
            velocity += frameVelocity;
            velocity.y = Mathf.Clamp(velocity.y, -90, 90);

            // Rotate camera up-down and controller left-right from velocity.
            camTransform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
            character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
        }
    }
}
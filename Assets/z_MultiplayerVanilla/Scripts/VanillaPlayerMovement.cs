using Mirror;
using UnityEngine;

namespace z_MultiplayerVanilla.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class VanillaPlayerMovement : NetworkBehaviour
    {
        private Transform target;
        private Rigidbody rb;

        [SerializeField] private float speed = 5;
        [SerializeField] private float runSpeed = 9;
        [SerializeField] private KeyCode runningKey = KeyCode.LeftShift;
        public bool canMove = true;
        
        [Header("Synced params")]
        [SyncVar] public bool IsRunning;
        [SyncVar] public Vector2 Velocity;

        private void Start()
        {
            target = transform;
            rb = GetComponent<Rigidbody>();
            rb.isKinematic = !isLocalPlayer;
            Debug.Log("Spawned, isLocalPlayer = " + isLocalPlayer);
        }

        private void FixedUpdate()
        {
            if (!isLocalPlayer || !canMove) return;

            IsRunning = canMove && Input.GetKey(runningKey);
            var targetMovingSpeed = IsRunning ? runSpeed : speed;
            Velocity = new Vector2(
                x: Input.GetAxis("Horizontal") * targetMovingSpeed,
                y: Input.GetAxis("Vertical") * targetMovingSpeed
            );
            rb.velocity = target.rotation * new Vector3(Velocity.x, rb.velocity.y, Velocity.y);
        }
    }
}
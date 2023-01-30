using Mirror;
using Multiplayer.Movement.domain;
using UnityEngine;
using Zenject;

namespace Multiplayer.Movement.presentation
{
    public class GroundCheck : NetworkBehaviour
    {
        [Inject] private IJumpingStateRepository jumpingStateRepository;
        
        public float maxGroundDistance = .3f;
        public bool isGrounded = true;
        public event System.Action Grounded;

        void Reset()
        {
            // if(!photonView.IsMine || !PhotonNetwork.IsConnected)
            //     return;
            //
            // transform.localPosition = Vector3.up * .01f;
        }

        void LateUpdate()
        {
            var isGroundedNow = Physics.Raycast(transform.position, Vector3.down, maxGroundDistance);
            if (isGroundedNow && !isGrounded)
                Grounded?.Invoke();
            isGrounded = isGroundedNow;
            jumpingStateRepository.SetJumping(!isGroundedNow);
        }

        void OnDrawGizmosSelected()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, maxGroundDistance))
                Debug.DrawLine(transform.position, hit.point, Color.white);
            else
                Debug.DrawLine(transform.position, transform.position + Vector3.down * maxGroundDistance, Color.red);
        }


        public static GroundCheck Create(Transform parent)
        {
            GameObject newGroundCheck = new GameObject("Ground check");
#if UNITY_EDITOR
            UnityEditor.Undo.RegisterCreatedObjectUndo(newGroundCheck, "Created ground check");
#endif
            newGroundCheck.transform.parent = parent;
            newGroundCheck.transform.localPosition = Vector3.up * .01f;
            return newGroundCheck.AddComponent<GroundCheck>();
        }
    }
}

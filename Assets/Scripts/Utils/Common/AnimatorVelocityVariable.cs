using UnityEngine;

namespace Utils.Misc
{
    public class AnimatorVelocityVariable : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private string param = "velocity";

        private Vector3 prevPos;
        // Update is called once per frame
        private void FixedUpdate()
        {
            var pos = rb.transform.position;
            var vel = prevPos - pos;
            animator.SetFloat(param, vel.magnitude);
            prevPos = pos;
        }
    }
}
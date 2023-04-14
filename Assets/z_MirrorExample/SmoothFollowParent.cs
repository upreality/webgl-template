using UnityEngine;

namespace MirrorExample
{
    public class SmoothFollowParent : MonoBehaviour
    {
        private Transform target;

        public bool isCustomOffset;
        public Vector3 offset;

        public float smoothSpeed = 0.1f;

        private void Start()
        {
            target = transform.parent;
            target.SetParent(null);

            if (!isCustomOffset)
                offset = transform.position - target.position;
        }

        private void LateUpdate()
        {
            DoSmoothFollow();
        }

        public void DoSmoothFollow()
        {
            Vector3 targetPos = target.position + offset;
            Vector3 smoothFollow = Vector3.Lerp(transform.position,
                targetPos, smoothSpeed);

            transform.position = smoothFollow;
            // transform.LookAt(target);
        }
    }
}
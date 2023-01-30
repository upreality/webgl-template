using UnityEngine;

namespace Utils.Misc
{
    public class RotationLimitedSync : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Transform source;
        [SerializeField] private Vector3 minAngle = -Vector3.one * 180;
        [SerializeField] private Vector3 maxMangle = Vector3.one * 180;
        [SerializeField] private bool useLocal = true;

        private void Start() => target ??= transform;

        private void LateUpdate()
        {
            var rotation = useLocal ? source.localRotation : source.rotation;
            var eulerRotation = rotation.eulerAngles;
            var clampedRotation = new Vector3(
                x: Mathf.Clamp(ClampAngle(eulerRotation.x), minAngle.x, maxMangle.x),
                y: Mathf.Clamp(ClampAngle(eulerRotation.y), minAngle.y, maxMangle.y),
                z: Mathf.Clamp(ClampAngle(eulerRotation.z), minAngle.z, maxMangle.z)
            );

            if (useLocal)
                target.localRotation = Quaternion.Euler(clampedRotation);
            else
                target.rotation = Quaternion.Euler(clampedRotation);
        }

        private static float ClampAngle(float angle)
        {
            angle %= 360;
            return angle > 180 ? angle - 360 : angle;
        }
    }
}
using UnityEngine;

namespace Utils.Misc
{
    public class DropRotation : MonoBehaviour
    {
        [SerializeField]
        private Vector3 defRot = Vector3.zero;
        private void Start()
        {
            transform.rotation = Quaternion.Euler(defRot);
        }
    }
}

using UnityEngine;

namespace Utils.Misc
{
    [RequireComponent(typeof(Animator))]
    public class CharacterIK : MonoBehaviour
    {
        private Animator animator;

        public Transform rightHandObj = null;
        public Transform leftHandObj = null;

        private void Start() => animator = GetComponent<Animator>();

        private void OnAnimatorIK(int layerIndex)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);

            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);
        }
    }
}
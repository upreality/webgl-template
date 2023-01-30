using System.Collections;
using UnityEngine;

namespace Utils.Misc
{
    public class Shake : MonoBehaviour
    {
        private Transform target;
        [SerializeField] private AnimationCurve lerpCurve;
        [SerializeField] private Vector3 startPos;
        [SerializeField] private Vector3 endPos;
        [SerializeField] private float duration;
        private float timer = 0f;

        private bool shaking = false;

        private void Start() => target = transform;

        public void AddShake()
        {
            if (shaking) timer = duration / 2f;
            else StartCoroutine(ShakeCoroutine());
        }

        private IEnumerator ShakeCoroutine()
        {
            shaking = true;
            timer = duration;
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                var lerpPosition = Mathf.Abs(timer / duration - 0.5f) * 2;
                transform.localPosition = Vector3.Lerp(startPos, endPos, lerpPosition);
                yield return null;
            }

            shaking = false;
        }

        private void OnDisable() => StopAllCoroutines();
    }
}
using System.Collections;
using UnityEngine;

namespace Utils.Misc
{
    public class MatBlink : MonoBehaviour
    {
        [SerializeField] private Renderer target;
        [SerializeField] private AnimationCurve lerpCurve;
        [SerializeField] private string prop;
        [SerializeField] private float startPos;
        [SerializeField] private float endPos;
        [SerializeField] private float duration;
        private float timer = 0f;

        private bool blinking = false;

        public void AddShake()
        {
            Debug.Log("Add Blink");
            if (blinking) timer = duration / 2f;
            else StartCoroutine(BlinkCoroutine());
        }

        private IEnumerator BlinkCoroutine()
        {
            blinking = true;
            timer = duration;
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                var lerpPosition = 1f - Mathf.Abs(timer / duration - 0.5f) * 2;
                target.material.SetFloat(prop, Mathf.Lerp(startPos, endPos, lerpPosition));
                yield return null;
            }
            target.sharedMaterial.SetFloat(prop, Mathf.Lerp(startPos, endPos, 0f));
            blinking = false;
        }

        private void OnDisable() => StopAllCoroutines();
    }
}

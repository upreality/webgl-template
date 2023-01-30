using System.Collections;
using UnityEngine;

namespace Utils.Misc
{
    [RequireComponent(typeof(LineRenderer))]
    public class TempLine : MonoBehaviour
    {
        private LineRenderer target;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float fadeDuration = 3f;
        [SerializeField] private float alphaMul = 1f;
        private float alpha = 1f;

        private void Start()
        {
            target = GetComponent<LineRenderer>();
            StartCoroutine(FadeLineRenderer());
        }

        IEnumerator FadeLineRenderer ()
        {
            var lineRendererGradient = new Gradient();
            var timeElapsed = 0f;
            while (timeElapsed < fadeDuration)
            {
                alpha = curve.Evaluate(timeElapsed / fadeDuration);
                lineRendererGradient.SetKeys
                (
                    target.colorGradient.colorKeys,
                    new[]
                    {
                        new GradientAlphaKey(0f, 0f),
                        new GradientAlphaKey(alpha * alphaMul, 0.5f),
                        new GradientAlphaKey(0f * alphaMul, 1f),
                    }
                );
                target.colorGradient = lineRendererGradient;
 
                timeElapsed += Time.deltaTime;
                yield return null;
            }
 
            Destroy(gameObject);
        }
    }
}

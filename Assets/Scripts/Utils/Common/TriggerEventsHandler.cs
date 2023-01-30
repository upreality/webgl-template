using UnityEngine;
using UnityEngine.Events;

namespace Utils.Misc
{
    public class TriggerEventsHandler : MonoBehaviour
    {
        [SerializeField] private string triggerTag = "Player";
        [SerializeField] private UnityEvent triggerEvent;
        [SerializeField] private UnityEvent exitTriggerEvent;
        [SerializeField] private bool triggerOnce;

        private bool triggeredEnter;
        private bool triggeredExit;

        private bool activeState;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(triggerTag) || (triggerOnce && triggeredEnter))
                return;

            triggerEvent?.Invoke();
            activeState = true;
            triggeredEnter = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(triggerTag) || (triggerOnce && triggeredExit))
                return;

            ExitTrigger();
            activeState = false;
            triggeredExit = true;
        }

        private void OnDestroy()
        {
            if (activeState)
                ExitTrigger();
        }

        private void ExitTrigger() => exitTriggerEvent?.Invoke();
    }
}
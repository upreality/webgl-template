using UnityEngine;
using Utils.Misc;

namespace Utils.Enviroment.Birds
{
    public class BirdAI : MonoBehaviour
    {
        private Transform me;
        private Transform target;
        [SerializeField] private BirdAIPointProvider Provider;
        [SerializeField] private SlerpToLookAt lookAt;
        [SerializeField] private float changePointDistance = 1f;

        void Start()
        {
            me = GetComponent<Transform>();
            UpdateTarget();
        }

        void UpdateTarget()
        {
            var next = Provider.GetNextTarget();
            while (next == target)
                next = Provider.GetNextTarget();
            target = next;
            lookAt.Target = target;
        }

        // Update is called once per frame
        void Update()
        {
            if ((target.position - me.position).magnitude >= changePointDistance)
                return;
            UpdateTarget();
        }
    }
}
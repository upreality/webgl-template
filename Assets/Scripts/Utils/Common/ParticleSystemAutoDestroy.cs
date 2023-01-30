using UnityEngine;

namespace Utils.Misc
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleSystemAutoDestroy : MonoBehaviour {
 
        private ParticleSystem _ps;
        public void Start()
        {
            _ps = GetComponent<ParticleSystem>();
        }
 
        public void FixedUpdate()
        {
            if (_ps && !_ps.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}
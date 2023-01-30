using Multiplayer.Damage.presentation.Damageable;
using UnityEngine;
using UnityEngine.Events;

namespace Multiplayer.Damage.presentation
{
    public class ProjectileDamager : MonoBehaviour
    {
        private Transform root;
        [SerializeField] private UnityEvent onAnyCollision = new();
        [SerializeField] private int damage;
        [SerializeField] private float speed;

        private void Start() => root = GetComponent<Transform>();

        private void FixedUpdate()
        {
            transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
            var ray = new Ray(root.position, root.forward);
            if (!Physics.Raycast(ray, out var hit)) return;
            HandleHit(hit);
            onAnyCollision.Invoke();
        }

        private void HandleHit(RaycastHit hit)
        {
            hit.transform.GetComponentInParent<IDamageable>()?.TakeDamage(damage);
        }
    }
}
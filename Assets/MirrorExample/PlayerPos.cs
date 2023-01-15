using Mirror;
using UnityEngine;

namespace MirrorExample
{
    public class PlayerPos : PredictionBehaviour<Vector2, Vector3>
    {
        [SerializeField] private Transform target;
        [SerializeField] private float radius;
        protected override Vector3 GetDefaultState() => Vector3.zero;

        protected override void Apply(Vector3 state)
        {
            target.position = state;
        }

        protected override Vector2 GetInput() => new(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical")
        );

        public override Vector3 Simulate(Vector3 state, Vector2 input)
        {
            var offset = new Vector3(
                x: input.x,
                y: 0f,
                z: input.y
            );
            var newState = state + offset;
            var ray = new Ray(transform.position, offset);
            if (!Physics.Raycast(ray, out var hit))
                return newState;

            var distance = hit.transform.position - transform.position;
            if (distance.magnitude < radius + offset.magnitude)
                return state + distance - distance.normalized * radius;

            return state + offset;
        }

        [Command]
        protected override void CmdOnInput(long timestamp, Vector2 input) => base.CmdOnInput(timestamp, input);

        [ClientRpc]
        protected override void RpcOnResult(long timestamp, Vector3 state) => base.RpcOnResult(timestamp, state);
    }
}
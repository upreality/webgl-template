using Mirror;
using UnityEngine;
using Utils.Reactive;

namespace MirrorExample
{
    public class PlayerPos2 : PredictionBehaviour2<PlayerPosState>
    {
        [SerializeField] protected int syncInputFrequencyMs = 50;
        [SerializeField] protected float speed = 5;

        private float inputX;
        private float inputY;

        private void Update()
        {
            if (isOwned)
            {
                inputX = Input.GetAxis("Horizontal");
                inputY = Input.GetAxis("Vertical");
            }

            transform.position += new Vector3(
                x: inputX * speed * Time.deltaTime,
                y: 0f,
                z: inputY * speed * Time.deltaTime
            );
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            this.CreateTimer(syncInputFrequencyMs, () => CmdSendInput(inputX, inputY));
        }

        protected override PlayerPosState GetState() => new(transform.position);

        protected override void SetState(PlayerPosState state) => transform.position = state.Position;

        [Command]
        private void CmdSendInput(float x, float y)
        {
            inputX = x;
            inputY = y;
        }

        [ClientRpc]
        protected override void RpcOnSync(double timestamp, PlayerPosState state) => base.RpcOnSync(timestamp, state);
    }

    public struct PlayerPosState : IState<PlayerPosState>
    {
        public Vector3 Position;

        public PlayerPosState(Vector3 position)
        {
            Position = position;
        }

        public PlayerPosState GetDelta(PlayerPosState state) => new(state.Position - Position);

        public PlayerPosState Multiply(float multiplier) => new(Position * multiplier);

        public PlayerPosState ApplyDelta(PlayerPosState delta) => new(Position + delta.Position);
    }
}
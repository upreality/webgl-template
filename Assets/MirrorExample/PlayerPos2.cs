using Mirror;
using UniRx;
using UnityEngine;

namespace MirrorExample
{
    public class PlayerPos2 : PredictionBehaviour2<PlayerPosState2>
    {
        [SerializeField] protected float speed = 5;

        private readonly Subject<Input> inputSubject = new();

        private void Start() => Observable
            .EveryUpdate()
            .CombineLatest(inputSubject, (l, input) => input)
            .Subscribe(ApplyInput)
            .AddTo(this);

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            inputSubject
                .DistinctUntilChanged()
                .Subscribe(CmdSendInput)
                .AddTo(this);
            Observable
                .EveryUpdate()
                .Select(_ => GetLocalInput())
                .DistinctUntilChanged()
                .Subscribe(input => inputSubject.OnNext(input))
                .AddTo(this);
        }

        protected override PlayerPosState2 GetState() => new(transform.position);

        protected override void SetState(PlayerPosState2 state)
        {
            base.SetState(state);
            // transform.position = state.Position;
        }

        private void ApplyInput(Input input)
        {
            transform.position += new Vector3(
                x: input.X * speed * Time.deltaTime,
                y: 0f,
                z: input.Y * speed * Time.deltaTime
            );
        }

        private static Input GetLocalInput() => new()
        {
            X = GetAxisInput("Horizontal"),
            Y = GetAxisInput("Vertical")
        };

        private static int GetAxisInput(string axisName)
        {
            var axisValue = UnityEngine.Input.GetAxis(axisName);
            if (Mathf.Abs(axisValue) < 0.001f)
                return 0;

            return axisValue > 0 ? 1 : -1;
        }

        [Command]
        private void CmdSendInput(Input input) => inputSubject.OnNext(input);

        [ClientRpc]
        protected override void RpcOnSync(double timestamp, PlayerPosState2 state) => base.RpcOnSync(timestamp, state);

        private struct Input
        {
            public int X;
            public int Y;
        }
    }
}
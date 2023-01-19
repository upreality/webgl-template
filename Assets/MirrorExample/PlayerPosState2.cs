using UnityEngine;

namespace MirrorExample
{
    public struct PlayerPosState2 : IState<PlayerPosState2>
    {
        public Vector3 Position;

        public PlayerPosState2(Vector3 position)
        {
            Position = position;
        }

        public PlayerPosState2 GetDelta(PlayerPosState2 state) => new(state.Position - Position);

        public PlayerPosState2 Multiply(float multiplier) => new(Position * multiplier);

        public PlayerPosState2 ApplyDelta(PlayerPosState2 delta) => new(Position + delta.Position);
    }
}
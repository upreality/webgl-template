namespace Multiplayer.Movement.domain.model
{
    public struct MovementState
    {
        public int XAxis;
        public int YAxis;
        public bool Jumping;

        public MovementState(int xAxis = 0, int yAxis = 0, bool jumping = false)
        {
            XAxis = xAxis;
            YAxis = yAxis;
            Jumping = jumping;
        }
    }
}
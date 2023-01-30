namespace Multiplayer.Movement.domain
{
    public interface IJumpingStateRepository
    {
        public bool IsJumping();
        internal void SetJumping(bool jumping);
    }
}
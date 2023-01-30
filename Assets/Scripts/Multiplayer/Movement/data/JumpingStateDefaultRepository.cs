using Multiplayer.Movement.domain;

namespace Multiplayer.Movement.data
{
    public class JumpingStateDefaultRepository: IJumpingStateRepository
    {
        private bool jump = false;
        
        public bool IsJumping()
        {
            return jump;
        }

        void IJumpingStateRepository.SetJumping(bool jumping)
        {
            jump = jumping;
        }
    }
}
using Multiplayer.PlayerInput.domain.model;
using Multiplayer.PlayerInput.domain.repositories;

namespace Multiplayer.PlayerInput.data.Repositories
{
    public class InputStateDefaultRepository : IInputStateRepository
    {
        private PlayerInputState state = PlayerInputState.Full;
        
        public PlayerInputState GetInputState() => state;

        void IInputStateRepository.SetInputState(PlayerInputState newState) => state = newState;
    }
}
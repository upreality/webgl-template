using Multiplayer.PlayerInput.domain.model;

namespace Multiplayer.PlayerInput.domain.repositories
{
    public interface IInputStateRepository
    {
        public PlayerInputState GetInputState();
        internal void SetInputState(PlayerInputState newState);
    }
}
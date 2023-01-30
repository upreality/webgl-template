using Multiplayer.PlayerInput.domain.model;
using Multiplayer.PlayerInput.domain.model.InputMask;

namespace Multiplayer.PlayerInput.domain.repositories
{
    public interface IInputMaskRepository
    {
        public IInputMask GetInputMask(PlayerInputState state);
    }
}
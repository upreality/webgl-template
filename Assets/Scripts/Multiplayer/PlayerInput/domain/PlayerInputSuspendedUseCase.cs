using Multiplayer.PlayerInput.domain.model;
using Multiplayer.PlayerInput.domain.repositories;
using Zenject;

namespace Multiplayer.PlayerInput.domain
{
    public class PlayerInputSuspendedUseCase
    {
        [Inject] private IInputStateRepository inputStateRepository;
        [Inject] private IInputMaskRepository inputMaskRepository;

        public bool GetAxisSuspended(PlayerInputAxis axis)
        {
            var state = inputStateRepository.GetInputState();
            return !inputMaskRepository.GetInputMask(state).InputAvailable(axis);
        }
        
        public bool GetNumericInputSuspended()
        {
            var state = inputStateRepository.GetInputState();
            return !inputMaskRepository.GetInputMask(state).NumericInputAvailable();
        }
    }
}
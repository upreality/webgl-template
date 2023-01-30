using Multiplayer.PlayerInput.domain.model;
using Multiplayer.PlayerInput.domain.repositories;
using Multiplayer.PlayerState.domain.model;
using Multiplayer.PlayerState.domain.repositories;
using Zenject;

namespace Multiplayer.PlayerInput.domain
{
    public class PlayerInputUseCase
    {
        [Inject] private IPlayerInputRepository inputRepository;
        [Inject] private IPlayerStateRepository playerStateRepository;
        [Inject] private PlayerInputSuspendedUseCase inputSuspendedUseCase;

        public float GetAxis(PlayerInputAxis axis) => InputAvailable(axis) ? 0f : inputRepository.GetAxis(axis);

        public bool GetSelection(out int selection)
        {
            var invalidPlayerState = playerStateRepository.GetPlayerState() != PlayerStates.Playing;
            if (!invalidPlayerState)
                return inputRepository.GetSelection(out selection);

            selection = 0;
            return false;
        }

        private bool InputAvailable(PlayerInputAxis axis)
        {
            var invalidPlayerState = playerStateRepository.GetPlayerState() != PlayerStates.Playing;
            var axisSuspended = inputSuspendedUseCase.GetAxisSuspended(axis);
            return invalidPlayerState || axisSuspended;
        }
    }
}
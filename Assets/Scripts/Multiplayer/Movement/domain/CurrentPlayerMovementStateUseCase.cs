using Multiplayer.Movement.domain.model;
using Multiplayer.PlayerInput.domain;
using Multiplayer.PlayerInput.domain.model;
using Zenject;

namespace Multiplayer.Movement.domain
{
    public class CurrentPlayerMovementStateUseCase
    {
        [Inject] private PlayerInputUseCase inputUseCase;
        [Inject] private IJumpingStateRepository jumpingStateRepository;

        public MovementState GetCurrentMovementState()
        {
            var xAxis = inputUseCase.GetAxis(PlayerInputAxis.HorizontalMovement);
            var yAxis = inputUseCase.GetAxis(PlayerInputAxis.VerticalMovement);
            
            return new MovementState(
                GetMovementStateAxisValue(xAxis),
                GetMovementStateAxisValue(yAxis),
                jumpingStateRepository.IsJumping()
            );
        }

        private int GetMovementStateAxisValue(float inputAxisValue)
        {
            if (inputAxisValue == 0f)
                return 0;

            var baseValue = inputAxisValue > 0f ? 1 : 0;
            var runningMul = inputUseCase.GetAxis(PlayerInputAxis.Running) > 0f ? 2 : 1;
            return baseValue * runningMul;
        }
    }
}
using Mirror;
using Multiplayer.Movement.domain;
using Multiplayer.Movement.domain.model;
using UniRx;
using UnityEngine;
using Zenject;

namespace Multiplayer.Movement.presentation
{
    public class AnimatorCharacterMovementPresenter: NetworkBehaviour
    {
        [Inject] private IMovementStateRepository movementStateRepository;
        
        [SerializeField] private Animator animator;
        [SerializeField] private string horAxis = "horizontal";
        [SerializeField] private string verAxis = "vertical";
        [SerializeField] private string isRunningAxis = "run";
        [SerializeField] private string isJumpingAxis = "jumping";
        
        private void Start()
        {
            // if (!this.GetPlayerId(out var userId))
                // return;
                var userId = "";
            
            movementStateRepository.GetMovementStateFlow(userId).Subscribe(ApplyState).AddTo(this);
        }
        private void ApplyState(MovementState state)
        {
            var isRunning = state.XAxis > 1 || state.YAxis > 1;
            animator.SetFloat(horAxis, state.XAxis);
            animator.SetFloat(verAxis, state.YAxis);
            animator.SetBool(isRunningAxis, isRunning);
            animator.SetBool(isJumpingAxis, state.Jumping);
        }
    }
}
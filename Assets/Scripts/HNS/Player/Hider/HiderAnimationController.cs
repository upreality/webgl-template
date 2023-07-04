using System;
using UniRx;
using UnityEngine;
using static HNS.Player.Hider.HiderAnimationController.HiderAnimationState;

namespace HNS.Player.Hider
{
    public class HiderAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator target;

        private readonly ISubject<HiderAnimationState> stateFlow = new BehaviorSubject<HiderAnimationState>(Undefined);

        public void SetAnimationState(HiderAnimationState state) => stateFlow.OnNext(state);
        
        private IDisposable animationHandler = Disposable.Empty;

        private void OnEnable() => animationHandler = stateFlow
            .DistinctUntilChanged()
            .Subscribe(HandleStateUpdate)
            .AddTo(this);

        private void OnDisable() => animationHandler.Dispose();

        private void HandleStateUpdate(HiderAnimationState state)
        {
            //TODO
            target.Play(state.ToString());
        }

        public enum HiderAnimationState
        {
            Undefined,
            Idle,
            Running,
            Hidden,
            Flying,
            Catched,
            Falling,
            Sleeping
        }
    }
}
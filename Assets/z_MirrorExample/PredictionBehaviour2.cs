using Mirror;
using UniRx;
using UnityEngine;
using Utils.Reactive;

namespace MirrorExample
{
    public abstract class PredictionBehaviour2<TState> : NetworkBehaviour where TState : IState<TState>
    {
        [SerializeField] protected int syncFrequencyMs = 100;
        [SerializeField] protected int cacheFrequencyMs = 50;

        private PredictionChain<TState> predictionChain;

        private TState CurrentState => GetState();

        public override void OnStartAuthority()
        {
            var initialState = GetState();
            predictionChain = new PredictionChain<TState>(initialState);
            predictionChain.LastState.Subscribe(SetState).AddTo(this);
            this.CreateTimer(cacheFrequencyMs, CacheClientState);
        }

        public override void OnStartServer() => this.CreateTimer(syncFrequencyMs, SyncToClient);

        private void CacheClientState()
        {
            predictionChain.Set(NetworkTime.time - NetworkTime.offset * 10, CurrentState);
        }

        private void SyncToClient() => RpcOnSync(NetworkTime.time, CurrentState);

        protected virtual void RpcOnSync(double timestamp, TState state)
        {
            CacheClientState();
            predictionChain.Set(timestamp, state);
            predictionChain.ResetFirstState(timestamp);
        }

        protected abstract TState GetState();

        protected virtual void SetState(TState state)
        {
            Debug.Log("UpdateLastState: " + state);
        }
        
        // public interface IInputDelayProvider
        // {
        //     long GetInputDelay();
        // }
    }
}
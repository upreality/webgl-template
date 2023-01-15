using System;
using System.Collections.Generic;
using Mirror;
using UniRx;
using UnityEngine;
using Utils.Misc;

namespace MirrorExample
{
    public abstract class NBTestBase<TInput, TState> : NetworkBehaviour, SimChain<TInput, TState>.ISimulator
    {
        [SerializeField] protected int simFreqMs = 100;

        private TState serverState;
        private long serverTimestamp;

        private readonly Queue<HandleInputRequest> inputQueue = new();

        private SimChain<TInput, TState> simChain;

        private long CurrentTimestamp => DateTime.Now.Ticks;

        private void Awake()
        {
            var defaultState = GetDefaultState();
            simChain = new SimChain<TInput, TState>(this, defaultState, CurrentTimestamp);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            serverState = GetDefaultState();
            var processInputTimerSpan = TimeSpan.FromMilliseconds(simFreqMs);
            Observable
                .Timer(processInputTimerSpan)
                .Repeat()
                .StartWith(0)
                .Subscribe(_ => HandleNextInputRequest())
                .AddTo(this);
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();
            var sendInputTimerSpan = TimeSpan.FromMilliseconds(simFreqMs);
            Observable
                .Timer(sendInputTimerSpan)
                .Repeat()
                .Subscribe(_ => SendInput())
                .AddTo(this);

            simChain
                .GetStateFlow()
                .Subscribe(Apply)
                .AddTo(this);
        }

        private void HandleNextInputRequest()
        {
            if (inputQueue.Count == 0)
                return;

            var nextRequest = inputQueue.Dequeue();
            serverState = Simulate(serverState, nextRequest.Input);
            HandleResult(nextRequest.Timestamp, serverState);
        }

        private void SendInput()
        {
            var input = GetInput();
            var timestamp = CurrentTimestamp;
            simChain.Simulate(timestamp, input);
            var inputBytes = ByteArrayUtils.ToByteArray(input);
            CmdHandleInput(timestamp, inputBytes);
        }

        protected virtual void CmdHandleInput(long timestamp, byte[] inputBytes)
        {
            if (timestamp < serverTimestamp)
                return;

            var input = ByteArrayUtils.FromByteArray<TInput>(inputBytes);
            var request = new HandleInputRequest(timestamp, input);
            inputQueue.Enqueue(request);
        }
        
        protected virtual void RpcHandleResult(long timestamp, byte[] bytesState)
        {
            var state = ByteArrayUtils.FromByteArray<TState>(bytesState);

            if (isOwned)
            {
                simChain.Apply(timestamp, state);
                return;
            }

            Apply(state);
        }

        private void HandleResult(long timestamp, TState state)
        {
            var bytesState = ByteArrayUtils.ToByteArray(state);
            RpcHandleResult(timestamp, bytesState);
        }

        protected abstract TState GetDefaultState();
        protected abstract void Apply(TState state);

        protected abstract TInput GetInput();
        public abstract TState Simulate(TState state, TInput input);

        private readonly struct HandleInputRequest
        {
            public readonly long Timestamp;
            public readonly TInput Input;

            public HandleInputRequest(long timestamp, TInput input)
            {
                Timestamp = timestamp;
                Input = input;
            }
        }
    }
}
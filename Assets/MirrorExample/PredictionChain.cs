using System;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UniRx;

namespace MirrorExample
{
    public class PredictionChain<TState> where TState : IState<TState>
    {
        private TState origin;

        private readonly Dictionary<double, TState> chain = new();

        private readonly BehaviorSubject<TState> lastStateSubject;

        public IObservable<TState> LastState => lastStateSubject;

        protected PredictionChain()
        {
            //hide default constructor
        }

        public PredictionChain(TState origin)
        {
            this.origin = origin;
            lastStateSubject = new BehaviorSubject<TState>(origin);
        }

        public void Set(double timestamp, TState state)
        {
            var timestamps = chain.Keys;
            if (timestamps.IsEmpty() || timestamp >= timestamps.Max())
            {
                chain[timestamp] = state;
                lastStateSubject.OnNext(state);
                return;
            }

            if (timestamp < timestamps.Min())
            {
                chain[timestamp] = state;
                return;
            }

            SetAndRecalculate(timestamp, state);
        }

        public void ResetFirstState(double timestamp)
        {
            var previousTimestamps = GetOrderedTimestamps(t => t > timestamp);

            if (previousTimestamps.Count < 2)
                return;

            var newOriginTimestamp = previousTimestamps[^2];
            origin = chain[newOriginTimestamp];
            for (var i = 0; i < previousTimestamps.Count - 1; i++)
            {
                chain.Remove(i);
            }
        }

        private TState Get(double timestamp)
        {
            if (chain.IsEmpty())
                return origin;

            if (chain.TryGetValue(timestamp, out var existingResult))
                return existingResult;

            var timestamps = chain.Keys;

            var minChainTimestamp = timestamps.Min();
            if (timestamp < minChainTimestamp)
                return origin;

            var maxChainTimestamp = timestamps.Max();
            if (timestamp >= maxChainTimestamp)
                return chain[maxChainTimestamp];

            var closestTimestamps = timestamps
                .OrderBy(chainTimestamp => Math.Abs(timestamp - chainTimestamp))
                .ToList();

            var fromTimestamp = closestTimestamps[0];
            var from = chain[fromTimestamp];

            var toTimestamp = closestTimestamps[1];
            var to = chain[toTimestamp];

            var lerpAmount = (timestamp - fromTimestamp) / (toTimestamp - fromTimestamp);
            var amount = (float)lerpAmount;
            var stateDelta = to.GetDelta(from).Multiply(amount);
            return from.ApplyDelta(stateDelta);
        }

        private void SetAndRecalculate(double timestamp, TState state)
        {
            var currentState = Get(timestamp);
            chain[timestamp] = state;
            var stateDelta = state.GetDelta(currentState);
            var invalidStateTimestamps = GetOrderedTimestamps(t => t > timestamp);

            var lastTimestamp = timestamp;
            foreach (var invalidTimestamp in invalidStateTimestamps)
            {
                var validState = chain[invalidTimestamp].ApplyDelta(stateDelta);
                chain[invalidTimestamp] = validState;
                lastTimestamp = invalidTimestamp;
            }

            var lastState = chain[lastTimestamp];
            lastStateSubject.OnNext(lastState);
        }

        private List<double> GetOrderedTimestamps(Func<double, bool> filter) => chain
            .Keys
            .Where(filter)
            .OrderBy(t => t)
            .ToList();
    }

    public interface IState<T>
    {
        T GetDelta(T state);
        T Multiply(float multiplier);
        T ApplyDelta(T delta);
    }
}
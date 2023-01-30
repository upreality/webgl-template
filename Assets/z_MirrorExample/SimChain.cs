using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ModestTree;
using UniRx;

namespace MirrorExample
{
    public class SimChain<TInput, TState>
    {
        private Simulation origin;

        private readonly Dictionary<long, SimulationResult> simulationChain = new();

        private readonly BehaviorSubject<TState> lastStateSubject;

        private readonly ISimulator simulator;

        private SimChain()
        {
            //disable default constructor
        }

        public SimChain(
            ISimulator simulator,
            TState originState,
            long originTimestamp
        )
        {
            this.simulator = simulator;
            origin = new Simulation(originTimestamp, originState);
            lastStateSubject = new BehaviorSubject<TState>(originState);
        }

        public void Simulate(long timestamp, TInput input)
        {
            var prevState = GetPreviousState(timestamp);
            var newState = simulator.Simulate(prevState, input);
            var result = new SimulationResult(input, newState);
            var simulation = new Simulation(timestamp, newState);

            simulationChain[timestamp] = result;
            RecalculateChainAfter(simulation);
        }

        public void Apply(long timestamp, TState state)
        {
            origin = new Simulation(timestamp, state);
            CleanChainBefore(timestamp);
            RecalculateChainAfter(origin);
        }

        public IObservable<TState> GetStateFlow() => lastStateSubject.DistinctUntilChanged();

        private void CleanChainBefore(long timestamp) => simulationChain
            .Keys
            .Where(chainTimestamp => chainTimestamp <= timestamp)
            .ToList()
            .ForEach(chainTimestamp => simulationChain.Remove(chainTimestamp));

        private void RecalculateChainAfter(Simulation simulation)
        {
            var followingSimulationTimestamps = simulationChain
                .Keys
                .Where(chainTimestamp => chainTimestamp > simulation.Timestamp)
                .OrderBy(chainTimestamp => chainTimestamp)
                .ToList();

            var state = simulation.State;
            foreach (var timestamp in followingSimulationTimestamps)
            {
                var currentSimulationInput = simulationChain[timestamp].Input;
                state = simulator.Simulate(state, currentSimulationInput);
                simulationChain[timestamp] = new SimulationResult(currentSimulationInput, state);
            }

            lastStateSubject.OnNext(state);
        }

        private TState GetPreviousState(long timestamp)
        {
            var previousChainTimestamps = simulationChain
                .Keys
                .Where(chainTimestamp => chainTimestamp < timestamp)
                .ToList();

            if (previousChainTimestamps.IsEmpty())
                return origin.State;

            var maxPreviousTimestamp = previousChainTimestamps.Max();
            return simulationChain[maxPreviousTimestamp].State;
        }

        /*
        private TState Get(long timestamp)
        {
            if (simulationChain.IsEmpty())
                return origin.State;

            if (simulationChain.TryGetValue(timestamp, out var existingResult))
                return existingResult.State;

            var chainTimestamps = simulationChain.Keys;
            if (origin.Timestamp >= timestamp)
                return origin.State;

            var minChainTimestamp = chainTimestamps.Min();
            if (timestamp < minChainTimestamp)
            {
                var minChainState = simulationChain[minChainTimestamp].State;
                var minChainSimulation = new Simulation(minChainTimestamp, minChainState);
                return LerpState(origin, minChainSimulation, timestamp);
            }

            var maxChainTimestamp = chainTimestamps.Max();
            if (timestamp >= maxChainTimestamp)
                return simulationChain[maxChainTimestamp].State;

            var closestTimestamps = chainTimestamps
                .OrderBy(chainTimestamp => Math.Abs(timestamp - chainTimestamp))
                .ToList();

            var fromTimestamp = closestTimestamps[0];
            var fromState = simulationChain[fromTimestamp].State;
            var from = new Simulation(fromTimestamp, fromState);

            var toTimestamp = closestTimestamps[1];
            var toState = simulationChain[toTimestamp].State;
            var to = new Simulation(toTimestamp, toState);

            return LerpState(from, to, timestamp);
        }

        private TState LerpState(Simulation from, Simulation to, long timestamp)
        {
            if (from.Timestamp > to.Timestamp)
                (from, to) = (to, from);

            var windowDuration = to.Timestamp - from.Timestamp;
            if (windowDuration == 0)
                return from.State;

            var progress = to.Timestamp - timestamp;
            if (progress < 0)
                return from.State;
            if (progress > windowDuration)
                return to.State;

            var t = (float)progress / windowDuration;
            return simulator.Lerp(from.State, to.State, t);
        }
        
        */

        public interface ISimulator
        {
            TState Simulate(TState source, TInput input);
            // TState Lerp(TState from, TState to, float t);
        }

        private class Simulation
        {
            public readonly long Timestamp;
            public readonly TState State;

            [SuppressMessage("ReSharper", "UnusedMember.Local")]
            private Simulation()
            {
                //default empty constructor
            }

            public Simulation(long timestamp, TState state)
            {
                Timestamp = timestamp;
                State = state;
            }
        }

        private struct SimulationResult
        {
            public readonly TInput Input;
            public readonly TState State;

            public SimulationResult(TInput input, TState state)
            {
                Input = input;
                State = state;
            }
        }
    }
}
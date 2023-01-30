using System;
using Multiplayer.Movement.domain;
using Multiplayer.Movement.domain.model;
using UniRx;
using Utils.Reactive;

namespace Multiplayer.Movement.data
{
    public class MovementStateDefaultRepository: IMovementStateRepository
    {
        private readonly ReactiveDictionary<string, MovementState> movableIdToStateMap = new();

        public MovementState GetMovementState(string movableID)=> movableIdToStateMap[movableID];

        public IObservable<MovementState> GetMovementStateFlow(string movableID) => movableIdToStateMap
            .GetItemFlow(movableID)
            .DistinctUntilChanged();

        void IMovementStateRepository.SetMovementState(string movableID, MovementState state) => movableIdToStateMap[movableID] = state;
    }
}
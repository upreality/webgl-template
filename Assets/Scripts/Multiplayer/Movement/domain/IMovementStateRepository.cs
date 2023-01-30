using System;
using Multiplayer.Movement.domain.model;

namespace Multiplayer.Movement.domain
{
    public interface IMovementStateRepository
    {
        public MovementState GetMovementState(string movableID);
        public IObservable<MovementState> GetMovementStateFlow(string movableID);
        internal void SetMovementState(string movableID, MovementState state);
    }
}
using Multiplayer.MatchState.domain.model;

namespace Multiplayer.MatchState.presentation.SyncHandler.model
{
    internal class RoomMatchStateParams
    {
        public readonly MatchStates CurrentState;
        public readonly int TimeLeft;

        public RoomMatchStateParams(MatchStates currentState, int timeLeft)
        {
            CurrentState = currentState;
            TimeLeft = timeLeft;
        }
    }
}
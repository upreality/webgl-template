using Multiplayer.MatchState.domain.model;

namespace Multiplayer.MatchState.domain.repositories
{
    public interface IMatchStateDurationRepository
    {
        bool GetStateDuration(MatchStates state, out int duration);
    }
}
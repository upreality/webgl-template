using Multiplayer.MatchState.domain.model;

namespace Multiplayer.MatchState.domain.repositories
{
    public interface IMatchStateDurationRepository
    {
        bool HasStateDuration(MatchStates state);
        bool GetStateDuration(MatchStates state, out int duration);
    }
}
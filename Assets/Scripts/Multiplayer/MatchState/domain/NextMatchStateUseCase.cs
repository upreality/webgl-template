using Multiplayer.MatchState.domain.model;

namespace Multiplayer.MatchState.domain
{
    public class NextMatchStateUseCase
    {
        public MatchStates GetNextMatchState(MatchStates current) => current switch
        {
            MatchStates.None => MatchStates.Preparing,
            MatchStates.Preparing => MatchStates.Playing,
            MatchStates.Playing => MatchStates.Finished,
            MatchStates.Finished => MatchStates.Preparing,
            _ => MatchStates.Playing
        };
    }
}
using Multiplayer.MatchState.domain.model;

namespace Multiplayer.MatchState.domain
{
    public class NextMatchStateUseCase
    {
        public MatchStates GetNextMatchState(MatchStates current) => current switch
        {
            MatchStates.Playing => MatchStates.Finished,
            _ => MatchStates.Playing
        };
    }
}
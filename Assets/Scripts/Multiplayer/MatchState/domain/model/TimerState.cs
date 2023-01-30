namespace Multiplayer.MatchState.domain.model
{
    public struct TimerState
    {
        public bool IsActive;
        public int TimeLeft;

        public TimerState(bool isActive, int timeLeft)
        {
            this.IsActive = isActive;
            this.TimeLeft = timeLeft;
        }
    }
}
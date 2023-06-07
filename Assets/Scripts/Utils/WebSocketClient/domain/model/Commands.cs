namespace Utils.WebSocketClient.domain.model
{
    public static class Commands
    {
        public const long LogIn = 0L;
        public const long Userdata = 1L;
        public const long LobbyAction = 2L;
        public const long LobbyState = 3L;
        public const long MatchState = 4L;
        public const long Balance = 5L;

        public const long FightFinished = 6L;
        public const long SetMovementData = 7L;
        public const long SetAttackDirection = 8L;
        public const long FightGameUpdate = 9L;
    }
}
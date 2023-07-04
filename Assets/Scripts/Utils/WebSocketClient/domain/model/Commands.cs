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

        public const long GameState = 6L;
        public const long RoundUpdates = 7L;
        public const long Movement = 8L;
        public const long Catch = 9L;
    }
}
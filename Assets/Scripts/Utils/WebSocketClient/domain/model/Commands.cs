namespace Utils.WebSocketClient.domain.model
{
    public static class Commands
    {
        public const long LogIn = 0L;
        public const long Userdata = 1L;
        public const long LobbyAction = 2L;
        public const long LobbyState = 3L;
        public const long MatchState = 4L;

        public const long TicTacState = 10L;
        public const long TicTacCellUpdates = 11L;
        public const long TicTacTurnUpdates = 12L;
        public const long TicTacMakeTurn = 13L;
        public const long TicTacFinished = 14L;
    }
}
namespace TexasHoldem.Logic.Players
{
    public struct PlayerActionAndName
    {
        public PlayerActionAndName(string playerName, PlayerAction action)
        {
            this.PlayerName = playerName;
            this.Action = action;
        }

        public string PlayerName { get; set; }

        public PlayerAction Action { get; set; }
    }
}

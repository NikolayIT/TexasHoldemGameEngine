namespace TexasHoldem.Logic.Players
{
    // TODO: Rename to PlayerAction
    public class PlayerTurn
    {
        private static readonly PlayerTurn FoldObject = new PlayerTurn(PlayerTurnType.Fold);
        private static readonly PlayerTurn CheckObject = new PlayerTurn(PlayerTurnType.Check);
        private static readonly PlayerTurn CallObject = new PlayerTurn(PlayerTurnType.Call);

        private PlayerTurn(PlayerTurnType type)
        {
            this.Type = type;
        }

        private PlayerTurn(int money)
        {
            this.Type = PlayerTurnType.Raise;
            this.Money = money;
        }

        public PlayerTurnType Type { get; }

        public int Money { get; }

        public static PlayerTurn Fold()
        {
            return FoldObject;
        }

        public static PlayerTurn Check()
        {
            return CheckObject;
        }

        public static PlayerTurn Call()
        {
            return CallObject;
        }

        public static PlayerTurn Raise(int money)
        {
            return new PlayerTurn(money);
        }
    }
}
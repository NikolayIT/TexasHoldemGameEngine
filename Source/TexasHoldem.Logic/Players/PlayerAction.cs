namespace TexasHoldem.Logic.Players
{
    public class PlayerAction
    {
        private static readonly PlayerAction FoldObject = new PlayerAction(PlayerActionType.Fold);
        private static readonly PlayerAction CheckObject = new PlayerAction(PlayerActionType.Check);
        private static readonly PlayerAction CallObject = new PlayerAction(PlayerActionType.Call);

        private PlayerAction(PlayerActionType type)
        {
            this.Type = type;
        }

        private PlayerAction(int money)
        {
            this.Type = PlayerActionType.Raise;
            this.Money = money;
        }

        public PlayerActionType Type { get; }

        public int Money { get; }

        public static PlayerAction Fold()
        {
            return FoldObject;
        }

        public static PlayerAction Check()
        {
            return CheckObject;
        }

        public static PlayerAction Call()
        {
            return CallObject;
        }

        public static PlayerAction Raise(int toAmount)
        {
            return new PlayerAction(toAmount);
        }
    }
}

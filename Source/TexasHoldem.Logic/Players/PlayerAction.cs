namespace TexasHoldem.Logic.Players
{
    public class PlayerAction
    {
        private static readonly PlayerAction FoldObject = new PlayerAction(PlayerActionType.Fold);
        private static readonly PlayerAction CheckCallObject = new PlayerAction(PlayerActionType.CheckCall);

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

        public static PlayerAction CheckOrCall()
        {
            return CheckCallObject;
        }

        public static PlayerAction Raise(int toAmount)
        {
            return new PlayerAction(toAmount);
        }

        public override string ToString()
        {
            return $"{this.Type}({this.Money})";
        }
    }
}

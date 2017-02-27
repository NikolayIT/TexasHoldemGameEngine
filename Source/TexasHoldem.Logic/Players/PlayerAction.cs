namespace TexasHoldem.Logic.Players
{
    using System;

    public class PlayerAction
    {
        private static readonly PlayerAction FoldObject = new PlayerAction(PlayerActionType.Fold);
        private static readonly PlayerAction CheckCallObject = new PlayerAction(PlayerActionType.CheckCall);

        private PlayerAction(PlayerActionType type)
        {
            this.Type = type;
        }

        private PlayerAction(int money, bool isAllIn = false)
        {
            this.Type = isAllIn ? PlayerActionType.AllIn : PlayerActionType.Raise;
            this.Money = money;
        }

        public PlayerActionType Type { get; }

        public int Money { get; internal set; }

        public static PlayerAction Fold()
        {
            return FoldObject;
        }

        public static PlayerAction CheckOrCall()
        {
            return CheckCallObject;
        }

        /// <summary>
        /// Creates a new object containing information about the player action and the raise amount
        /// </summary>
        /// <param name="withAmount">
        /// The amount to raise with.
        /// If amount is less than the minimum amount for raising then the game will take this minimum amount from the players money.
        /// </param>
        /// <returns>A new player action object containing information about the player action and the raise amount</returns>
        public static PlayerAction Raise(int withAmount)
        {
            if (withAmount <= 0)
            {
                return CheckOrCall();
            }

            return new PlayerAction(withAmount);
        }

        /// <summary>
        /// Creates a new object containing information about the player action.
        /// </summary>
        /// <param name="moneyLeft">
        /// The amount of money player has left to all-in with.
        /// If amount is less than the minimum amount for raising then the game will take this minimum amount from the players money.
        /// </param>
        /// <returns>A new player action object containing information about the player action and the raise amount</returns>
        public static PlayerAction AllIn(int moneyLeft)
        {
            if (moneyLeft <= 0)
            {
                return CheckOrCall();
            }

            return new PlayerAction(moneyLeft, true);
        }

        public override string ToString()
        {
            if (this.Type == PlayerActionType.Raise)
            {
                return $"{this.Type}({this.Money})";
            }
            else
            {
                return this.Type.ToString();
            }
        }
    }
}

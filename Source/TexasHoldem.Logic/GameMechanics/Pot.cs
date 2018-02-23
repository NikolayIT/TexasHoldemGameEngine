namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;

    public struct Pot
    {
        public Pot(int amountOfMoney, IReadOnlyList<string> activePlayer)
        {
            this.AmountOfMoney = amountOfMoney;
            this.ActivePlayer = activePlayer;
        }

        public int AmountOfMoney { get; }

        public IReadOnlyList<string> ActivePlayer { get; }
    }
}

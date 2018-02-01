namespace TexasHoldem.Logic.Players
{
    using System;
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;

    public struct Opponent
    {
        public Opponent(string name, int position, ICollection<Card> holeCards, bool inHand, int moneyLeft)
        {
            this.Name = name;
            this.Position = position;
            this.HoleCards = holeCards;
            this.InHand = inHand;
            this.MoneyLeft = moneyLeft;
        }

        public string Name { get; }

        public int Position { get; }

        public ICollection<Card> HoleCards { get; }

        public bool InHand { get; }

        public int MoneyLeft { get; }
    }
}

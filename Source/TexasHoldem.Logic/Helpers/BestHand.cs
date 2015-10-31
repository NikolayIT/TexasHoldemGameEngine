namespace TexasHoldem.Logic.Helpers
{
    using System;
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;

    public class BestHand : IComparable<BestHand>
    {
        internal BestHand(HandRankType rankType, ICollection<CardType> cards)
        {
            this.Cards = cards;
            this.RankType = rankType;
        }

        // When comparing or ranking cards, the suit doesn't matter
        public ICollection<CardType> Cards { get; }

        public HandRankType RankType { get; }

        public int CompareTo(BestHand other)
        {
            if (this.RankType > other.RankType)
            {
                return 1;
            }

            if (this.RankType < other.RankType)
            {
                return -1;
            }

            // TODO: What if same rank type?
            return 0;
        }
    }
}

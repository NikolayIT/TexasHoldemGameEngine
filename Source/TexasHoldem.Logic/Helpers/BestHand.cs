namespace TexasHoldem.Logic.Helpers
{
    using System;
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;

    public class BestHand : IComparable<BestHand>
    {
        internal BestHand(HandRankType rankType, IList<CardType> cards)
        {
            if (cards.Count != 5)
            {
                throw new ArgumentException("Cards collection should contains exactly 5 elements", nameof(cards));
            }

            this.Cards = cards;
            this.RankType = rankType;
        }

        // When comparing or ranking cards, the suit doesn't matter
        public IList<CardType> Cards { get; }

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

            for (var i = 0; i < this.Cards.Count; i++)
            {
                // Not sure if much sense of this.
                if (this.Cards[i] == other.Cards[i])
                {
                    continue;
                }

                if (this.Cards[i] > other.Cards[i])
                {
                    return 1;
                }

                if (this.Cards[i] < other.Cards[i])
                {
                    return -1;
                }
            }

            return 0;
        }
    }
}

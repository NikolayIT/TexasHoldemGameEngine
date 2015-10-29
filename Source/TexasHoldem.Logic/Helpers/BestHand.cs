namespace TexasHoldem.Logic.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Cards;

    public class BestHand : IComparable<BestHand>
    {
        public BestHand(HandRankType rankType)
            : this(rankType, new List<CardType>())
        {
            this.RankType = rankType;
        }

        public BestHand(HandRankType rankType, IEnumerable<CardType> cards)
        {
            this.Cards = cards;
            this.RankType = rankType;
        }

        // When comparing or ranking cards, the suit doesn't matter
        public IEnumerable<CardType> Cards { get; }

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
            throw new NotImplementedException();

            return 0;
        }
    }
}

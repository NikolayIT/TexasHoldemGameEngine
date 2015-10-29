namespace TexasHoldem.Logic.Helpers
{
    using System;
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;

    public class BestHand : IComparable<BestHand>
    {
        public BestHand(HandRankType rankType)
            : this(rankType, new List<Card>())
        {
            this.RankType = rankType;
        }

        public BestHand(HandRankType rankType, ICollection<Card> cards)
        {
            this.Cards = cards;
            this.RankType = rankType;
        }

        public ICollection<Card> Cards { get; }

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

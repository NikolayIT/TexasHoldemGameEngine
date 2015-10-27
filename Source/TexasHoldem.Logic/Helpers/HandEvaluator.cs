namespace TexasHoldem.Logic.Helpers
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;

    // TODO: Return more information (e.g. high card, which pairs the user has, what is the highest card in the straight, etc.)
    // For performance considerations this class is not implemented using Chain of Responsibility
    public class HandEvaluator
    {
        public HandRankType GetRankType(ICollection<Card> cards)
        {
            var hasStraight = this.HasStraight(cards);
            var hasFlush = this.HasFlush(cards);
            if (hasStraight && hasFlush)
            {
                return HandRankType.StraightFlush;
            }

            var hasFourOfAKind = this.HasFourOfAKind(cards);
            if (hasFourOfAKind)
            {
                return HandRankType.FourOfAKind;
            }

            var pairsCount = this.PairsCount(cards);
            var hasThreeOfAKind = this.HasThreeOfAKind(cards);
            if (pairsCount > 0 && hasThreeOfAKind)
            {
                return HandRankType.FullHouse;
            }

            if (hasFlush)
            {
                return HandRankType.Flush;
            }

            if (hasStraight)
            {
                return HandRankType.Straight;
            }

            if (hasThreeOfAKind)
            {
                return HandRankType.ThreeOfAKind;
            }

            if (pairsCount >= 2)
            {
                return HandRankType.TwoPairs;
            }

            if (pairsCount > 0)
            {
                return HandRankType.Pair;
            }

            return HandRankType.HighCard;
        }

        private int PairsCount(ICollection<Card> cards)
        {
            // TODO: Implement
            return 0;
        }

        private bool HasFourOfAKind(ICollection<Card> cards)
        {
            // TODO: Implement
            return false;
        }

        private bool HasThreeOfAKind(ICollection<Card> cards)
        {
            // TODO: Implement
            return false;
        }

        private bool HasStraight(ICollection<Card> cards)
        {
            // TODO: Implement
            return false;
        }

        private bool HasFlush(ICollection<Card> cards)
        {
            // TODO: Implement
            return false;
        }
    }
}

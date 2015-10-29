namespace TexasHoldem.Logic.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Cards;

    // TODO: Return more information (e.g. high card, which pairs the user has, what is the highest card in the straight, etc.)
    // TODO: Consider replacing LINQ with something more efficient
    // For performance considerations this class is not implemented using Chain of Responsibility
    public class HandEvaluator
    {
        public BestHand GetBestHand(ICollection<Card> cards)
        {
            if (this.HasStraightFlush(cards))
            {
                return new BestHand(HandRankType.StraightFlush);
            }

            if (this.HasFourOfAKind(cards))
            {
                return new BestHand(HandRankType.FourOfAKind);
            }

            var pairsCount = this.PairsCount(cards);
            var hasThreeOfAKind = this.HasThreeOfAKind(cards);
            if (pairsCount > 0 && hasThreeOfAKind)
            {
                return new BestHand(HandRankType.FullHouse);
            }

            if (this.HasFlush(cards))
            {
                return new BestHand(HandRankType.Flush);
            }

            if (this.HasStraight(cards))
            {
                return new BestHand(HandRankType.Straight);
            }

            if (hasThreeOfAKind)
            {
                return new BestHand(HandRankType.ThreeOfAKind);
            }

            if (pairsCount >= 2)
            {
                return new BestHand(HandRankType.TwoPairs);
            }

            if (pairsCount > 0)
            {
                return new BestHand(HandRankType.Pair);
            }

            return new BestHand(HandRankType.HighCard);
        }

        private int PairsCount(ICollection<Card> cards)
        {
            return cards.GroupBy(x => x.Type).Count(x => x.Count() == 2);
        }

        private bool HasFourOfAKind(ICollection<Card> cards)
        {
            return cards.GroupBy(x => x.Type).Any(x => x.Count() == 4);
        }

        private bool HasThreeOfAKind(ICollection<Card> cards)
        {
            return cards.GroupBy(x => x.Type).Any(x => x.Count() == 3);
        }

        private bool HasStraightFlush(ICollection<Card> cards)
        {
            var flushes = cards.GroupBy(x => x.Suit).Where(x => x.Count() >= 5).Select(x => x.ToList());
            return flushes.Any(this.HasStraight);
        }

        private bool HasStraight(ICollection<Card> cards)
        {
            var types = cards.GroupBy(x => x.Type).Select(x => (int)x.Key).ToList();
            if (cards.Any(x => x.Type == CardType.Ace))
            {
                types.Add((int)CardType.Two - 1);
            }

            types.Sort();

            var longestSequence = 0;
            var currentSequence = 0;
            var lastType = int.MaxValue;
            foreach (var type in types)
            {
                if (type - 1 == lastType)
                {
                    currentSequence++;
                }
                else
                {
                    currentSequence = 1;
                }

                lastType = type;
                longestSequence = Math.Max(longestSequence, currentSequence);
            }

            return longestSequence >= 5;
        }

        private bool HasFlush(ICollection<Card> cards)
        {
            return cards.GroupBy(x => x.Suit).Any(x => x.Count() >= 5);
        }
    }
}

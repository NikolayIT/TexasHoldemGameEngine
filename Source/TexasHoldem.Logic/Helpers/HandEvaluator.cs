namespace TexasHoldem.Logic.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Cards;

    // TODO: Consider replacing LINQ with something more efficient (profile the code)
    // For performance considerations this class is not implemented using Chain of Responsibility
    public class HandEvaluator
    {
        public BestHand GetBestHand(ICollection<Card> cards)
        {
            if (this.HasStraightFlush(cards))
            {
                return new BestHand(HandRankType.StraightFlush, new List<CardType>());
            }

            if (this.HasFourOfAKind(cards))
            {
                return new BestHand(HandRankType.FourOfAKind, new List<CardType>());
            }

            var pairTypes = this.GetPairTypes(cards);
            var hasThreeOfAKind = this.HasThreeOfAKind(cards);
            if (pairTypes.Any() && hasThreeOfAKind)
            {
                return new BestHand(HandRankType.FullHouse, new List<CardType>());
            }

            if (this.HasFlush(cards))
            {
                return new BestHand(HandRankType.Flush, new List<CardType>());
            }

            if (this.HasStraight(cards))
            {
                return new BestHand(HandRankType.Straight, new List<CardType>());
            }

            if (hasThreeOfAKind)
            {
                return new BestHand(HandRankType.ThreeOfAKind, new List<CardType>());
            }

            if (pairTypes.Count >= 2)
            {
                var bestCards =
                    cards.Where(x => x.Type != pairTypes[0] && x.Type != pairTypes[1])
                        .OrderByDescending(x => x.Type)
                        .Select(x => x.Type)
                        .Take(1).ToList();
                bestCards.Add(pairTypes[0]);
                bestCards.Add(pairTypes[0]);
                bestCards.Add(pairTypes[1]);
                bestCards.Add(pairTypes[1]);
                return new BestHand(HandRankType.TwoPairs, bestCards);
            }

            if (pairTypes.Count == 1)
            {
                var bestCards =
                    cards.Where(x => x.Type != pairTypes[0])
                        .OrderByDescending(x => x.Type)
                        .Select(x => x.Type)
                        .Take(3).ToList();
                bestCards.Add(pairTypes[0]);
                bestCards.Add(pairTypes[0]);
                return new BestHand(HandRankType.Pair, bestCards);
            }
            else
            {
                var bestCards = cards.OrderByDescending(x => x.Type).Select(x => x.Type).Take(5);
                return new BestHand(HandRankType.HighCard, bestCards);
            }
        }

        private IList<CardType> GetPairTypes(IEnumerable<Card> cards)
        {
            return cards.GroupBy(x => x.Type).Where(x => x.Count() == 2).Select(x => x.Key).OrderByDescending(x => x).ToList();
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

﻿namespace TexasHoldem.Logic.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Cards;

    // TODO: Consider replacing LINQ with something more efficient (profile the code)
    // For performance considerations this class is not implemented using Chain of Responsibility
    public class HandEvaluator : IHandEvaluator
    {
        private const int ComparableCards = 5;

        public BestHand GetBestHand(ICollection<Card> cards)
        {
            var straigtFlushCards = this.GetStraightFlushCards(cards);
            if (straigtFlushCards != null)
            {
                return new BestHand(HandRankType.StraightFlush, straigtFlushCards);
            }

            if (this.HasFourOfAKind(cards))
            {
                var fourOfAKindType = cards.GroupBy(x => x.Type).Where(x => x.Count() == 4).Select(x => x.Key).FirstOrDefault();
                var bestCards = cards.Where(x => x.Type != fourOfAKindType)
                    .OrderByDescending(x => x.Type)
                    .Select(x => x.Type)
                    .Take(ComparableCards - 4).ToList();
                bestCards.AddRange(Enumerable.Repeat(fourOfAKindType, 4));
                bestCards.Reverse();

                return new BestHand(HandRankType.FourOfAKind, bestCards);
            }

            var pairTypes = this.GetPairTypes(cards);
            var threeOfAKindTypes = this.GetThreeOfAKinds(cards);
            if ((pairTypes.Count > 0 && threeOfAKindTypes.Count > 0) || threeOfAKindTypes.Count == 2)
            {
                var bestCards = new List<CardType>();
                if (pairTypes.Count > 0)
                {
                    bestCards.AddRange(Enumerable.Repeat(threeOfAKindTypes[0], 3));
                    bestCards.AddRange(Enumerable.Repeat(pairTypes[0], 2));
                }
                else if (threeOfAKindTypes.Count == 2)
                {
                    bestCards.AddRange(Enumerable.Repeat(threeOfAKindTypes[0], 3));
                    bestCards.AddRange(Enumerable.Repeat(threeOfAKindTypes[1], 2));
                }

                return new BestHand(HandRankType.FullHouse, bestCards);
            }

            var flushCards = this.GetFlushCards(cards);
            if (flushCards != null)
            {
                return new BestHand(HandRankType.Flush, flushCards);
            }

            var straightCards = this.GetStraightCards(cards);
            if (straightCards != null)
            {
                return new BestHand(HandRankType.Straight, straightCards);
            }

            if (threeOfAKindTypes.Count > 0)
            {
                var bestThreeOfAKindType = threeOfAKindTypes[0];
                var bestCards =
                    cards.Where(x => x.Type != bestThreeOfAKindType)
                        .OrderBy(x => x.Type)
                        .Select(x => x.Type)
                        .Skip(cards.Count - 2)
                        .Take(ComparableCards - 3).ToList();
                bestCards.AddRange(Enumerable.Repeat(bestThreeOfAKindType, 3));
                bestCards.Reverse();

                return new BestHand(HandRankType.ThreeOfAKind, bestCards);
            }

            if (pairTypes.Count >= 2)
            {
                var bestCards =
                    cards.Where(x => x.Type != pairTypes[0] && x.Type != pairTypes[1])
                        .OrderByDescending(x => x.Type)
                        .Select(x => x.Type)
                        .Take(ComparableCards - 4).ToList();
                bestCards.Add(pairTypes[1]);
                bestCards.Add(pairTypes[1]);
                bestCards.Add(pairTypes[0]);
                bestCards.Add(pairTypes[0]);
                bestCards.Reverse();

                return new BestHand(HandRankType.TwoPairs, bestCards);
            }

            if (pairTypes.Count == 1)
            {
                var bestCards =
                    cards.Where(x => x.Type != pairTypes[0])
                        .OrderBy(x => x.Type)
                        .Select(x => x.Type)
                        .Skip(cards.Count - 3)
                        .Take(ComparableCards - 2).ToList();
                bestCards.Add(pairTypes[0]);
                bestCards.Add(pairTypes[0]);
                bestCards.Reverse();

                return new BestHand(HandRankType.Pair, bestCards);
            }
            else
            {
                var bestCards = cards.OrderByDescending(x => x.Type).Select(x => x.Type).Take(ComparableCards).ToList();
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

        private IList<CardType> GetThreeOfAKinds(ICollection<Card> cards)
        {
            return cards.GroupBy(x => x.Type).Where(x => x.Count() == 3).Select(x => x.Key).OrderByDescending(x => x).ToList();
        }

        private IList<CardType> GetStraightFlushCards(ICollection<Card> cards)
        {
            var flush = cards.GroupBy(x => x.Suit).First(x => x.Count() >= ComparableCards).ToList();

            return flush == null ? null : this.GetStraightCards(flush);
        }

        private IList<CardType> GetStraightCards(ICollection<Card> cards)
        {
            var types = cards.GroupBy(x => x.Type).Select(x => (int)x.Key).OrderByDescending(x => x).ToList();
            if (types[0] == (int)CardType.Ace)
            {
                types.Add((int)CardType.Two - 1);
            }

            var cardsCount = types.Count;
            var currentSequence = 1;
            var straightCards = new List<CardType>();
            var lastType = types[0];
            straightCards.Add((CardType)lastType);
            for (var i = 1; i < cardsCount; i++)
            {
                if (types[i] - 1 == lastType)
                {
                    currentSequence++;
                    straightCards.Add((CardType)types[i]);
                    if (currentSequence >= ComparableCards)
                    {
                        return straightCards;
                    }
                }
                else
                {
                    if (i > cardsCount - ComparableCards)
                    {
                        return null;
                    }

                    straightCards.Clear();
                    straightCards.Add((CardType)types[i]);
                    currentSequence = 1;
                }

                lastType = types[i];
            }

            return null;
        }

        private IList<CardType> GetFlushCards(ICollection<Card> cards)
        {
            var flushCardTypes = cards
                .GroupBy(x => x.Suit)
                .FirstOrDefault(x => x.Count() >= ComparableCards)
                ?.Select(x => x.Type)
                .OrderByDescending(x => x)
                .Take(ComparableCards)
                .ToList();

            return flushCardTypes;
        }
    }
}

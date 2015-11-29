namespace TexasHoldem.Logic.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Cards;

    // For performance considerations this class is not implemented using Chain of Responsibility
    public class HandEvaluator : IHandEvaluator
    {
        private const int ComparableCards = 5;

        /// <summary>
        /// Finds the best possible hand given a player's cards and all revealed comunity cards.
        /// </summary>
        /// <param name="cards">A player's cards + all revealed comunity cards (at lesat 5 in total)</param>
        /// <returns>Returns an object of type BestHand</returns>
        public BestHand GetBestHand(IEnumerable<Card> cards)
        {
            var cardSuitCounts = new int[(int)CardSuit.Spade + 1];
            var cardTypeCounts = new int[(int)CardType.Ace + 1];
            foreach (var card in cards)
            {
                cardSuitCounts[(int)card.Suit]++;
                cardTypeCounts[(int)card.Type]++;
            }

            // Flushes
            if (cardSuitCounts.Any(x => x >= ComparableCards))
            {
                // Straight flush
                var straightFlushCards = this.GetStraightFlushCards(cardSuitCounts, cards);
                if (straightFlushCards.Count > 0)
                {
                    return new BestHand(HandRankType.StraightFlush, straightFlushCards);
                }

                // Flush - it is not possible to have Flush and either Four of a kind or Full house at the same time
                for (var i = 0; i < cardSuitCounts.Length; i++)
                {
                    if (cardSuitCounts[i] >= ComparableCards)
                    {
                        var flushCards =
                            cards.Where(x => x.Suit == (CardSuit)i)
                                .Select(x => x.Type)
                                .OrderByDescending(x => x)
                                .Take(ComparableCards)
                                .ToList();
                        return new BestHand(HandRankType.Flush, flushCards);
                    }
                }
            }

            // Four of a kind
            if (cardTypeCounts.Any(x => x == 4))
            {
                var bestFourOfAKind = this.GetTypesWithNCards(cardTypeCounts, 4)[0];
                var bestCards = new List<CardType>
                                    {
                                        bestFourOfAKind,
                                        bestFourOfAKind,
                                        bestFourOfAKind,
                                        bestFourOfAKind,
                                        cards.Where(x => x.Type != bestFourOfAKind).Max(x => x.Type)
                                    };

                return new BestHand(HandRankType.FourOfAKind, bestCards);
            }

            // Full
            var pairTypes = this.GetTypesWithNCards(cardTypeCounts, 2);
            var threeOfAKindTypes = this.GetTypesWithNCards(cardTypeCounts, 3);
            if ((pairTypes.Count > 0 && threeOfAKindTypes.Count > 0) || threeOfAKindTypes.Count > 1)
            {
                var bestCards = new List<CardType>();
                for (var i = 0; i < 3; i++)
                {
                    bestCards.Add(threeOfAKindTypes[0]);
                }

                if (threeOfAKindTypes.Count > 1)
                {
                    for (var i = 0; i < 2; i++)
                    {
                        bestCards.Add(threeOfAKindTypes[1]);
                    }
                }

                if (pairTypes.Count > 0)
                {
                    for (var i = 0; i < 2; i++)
                    {
                        bestCards.Add(pairTypes[0]);
                    }
                }

                return new BestHand(HandRankType.FullHouse, bestCards);
            }

            // Straight
            var straightCards = this.GetStraightCards(cardTypeCounts);
            if (straightCards != null)
            {
                return new BestHand(HandRankType.Straight, straightCards);
            }

            // 3 of a kind
            if (threeOfAKindTypes.Count > 0)
            {
                var bestThreeOfAKindType = threeOfAKindTypes[0];
                var bestCards =
                    cards.Where(x => x.Type != bestThreeOfAKindType)
                        .Select(x => x.Type)
                        .OrderByDescending(x => x)
                        .Take(ComparableCards - 3).ToList();
                bestCards.AddRange(Enumerable.Repeat(bestThreeOfAKindType, 3));

                return new BestHand(HandRankType.ThreeOfAKind, bestCards);
            }

            // Two pairs
            if (pairTypes.Count >= 2)
            {
                var bestCards = new List<CardType>
                                    {
                                        pairTypes[0],
                                        pairTypes[0],
                                        pairTypes[1],
                                        pairTypes[1],
                                        cards.Where(x => x.Type != pairTypes[0] && x.Type != pairTypes[1])
                                            .Max(x => x.Type)
                                    };
                return new BestHand(HandRankType.TwoPairs, bestCards);
            }

            // Pair
            if (pairTypes.Count == 1)
            {
                var bestCards =
                    cards.Where(x => x.Type != pairTypes[0])
                        .Select(x => x.Type)
                        .OrderByDescending(x => x)
                        .Take(3).ToList();
                bestCards.Add(pairTypes[0]);
                bestCards.Add(pairTypes[0]);
                return new BestHand(HandRankType.Pair, bestCards);
            }
            else
            {
                // High card
                var bestCards = new List<CardType>();
                for (var i = cardTypeCounts.Length - 1; i >= 0; i--)
                {
                    if (cardTypeCounts[i] > 0)
                    {
                        bestCards.Add((CardType)i);
                    }

                    if (bestCards.Count == ComparableCards)
                    {
                        break;
                    }
                }

                return new BestHand(HandRankType.HighCard, bestCards);
            }
        }

        private IList<CardType> GetTypesWithNCards(int[] cardTypeCounts, int n)
        {
            var pairs = new List<CardType>();
            for (var i = cardTypeCounts.Length - 1; i >= 0; i--)
            {
                if (cardTypeCounts[i] == n)
                {
                    pairs.Add((CardType)i);
                }
            }

            return pairs;
        }

        private ICollection<CardType> GetStraightFlushCards(int[] cardSuitCounts, IEnumerable<Card> cards)
        {
            var straightFlushCardTypes = new List<CardType>();
            for (var i = 0; i < cardSuitCounts.Length; i++)
            {
                if (cardSuitCounts[i] < ComparableCards)
                {
                    continue;
                }

                var cardTypeCounts = new int[(int)CardType.Ace + 1];
                foreach (var card in cards)
                {
                    if (card.Suit == (CardSuit)i)
                    {
                        cardTypeCounts[(int)card.Type]++;
                    }
                }

                var bestStraight = this.GetStraightCards(cardTypeCounts);
                if (bestStraight != null)
                {
                    straightFlushCardTypes.AddRange(bestStraight);
                }
            }

            return straightFlushCardTypes;
        }

        private ICollection<CardType> GetStraightCards(int[] cardTypeCounts)
        {
            var lastCardType = cardTypeCounts.Length;
            var straightLength = 0;
            for (var i = cardTypeCounts.Length - 1; i >= 1; i--)
            {
                var hasCardsOfType = cardTypeCounts[i] > 0 || (i == 1 && cardTypeCounts[(int)CardType.Ace] > 0);
                if (hasCardsOfType && i == lastCardType - 1)
                {
                    straightLength++;
                    if (straightLength == ComparableCards)
                    {
                        var bestStraight = new List<CardType>(ComparableCards);
                        for (var j = i; j <= i + ComparableCards - 1; j++)
                        {
                            if (j == 1)
                            {
                                bestStraight.Add(CardType.Ace);
                            }
                            else
                            {
                                bestStraight.Add((CardType)j);
                            }
                        }

                        return bestStraight;
                    }
                }
                else
                {
                    straightLength = 0;
                }

                lastCardType = i;
            }

            return null;
        }
    }
}
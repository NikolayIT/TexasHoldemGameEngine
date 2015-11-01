namespace TexasHoldem.Logic.Helpers
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
            var cardTypeCounts = new int[15]; // Ace = 14
            foreach (var card in cards)
            {
                cardTypeCounts[(int)card.Type]++;
            }

            var cardSuitCounts = new int[4]; // 0, 1, 2, 3
            foreach (var card in cards)
            {
                cardSuitCounts[(int)card.Suit]++;
            }

            // Straight flush
            var straightFlushCards = this.GetStraightFlushCards(cardSuitCounts, cards);
            if (straightFlushCards.Count > 0)
            {
                return new BestHand(HandRankType.StraightFlush, straightFlushCards);
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

            // Flush
            var flushCards = this.GetFlushCards(cards);
            if (flushCards != null)
            {
                return new BestHand(HandRankType.Flush, flushCards);
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
                        .OrderByDescending(x => x.Type)
                        .Select(x => x.Type)
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
                        .OrderByDescending(x => x.Type)
                        .Select(x => x.Type)
                        .Take(3).ToList();
                bestCards.Add(pairTypes[0]);
                bestCards.Add(pairTypes[0]);
                return new BestHand(HandRankType.Pair, bestCards);
            }
            else
            {
                // High card
                var bestCards = cards.OrderByDescending(x => x.Type).Select(x => x.Type).Take(ComparableCards).ToList();
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

        private ICollection<CardType> GetStraightFlushCards(int[] cardSuitCounts, ICollection<Card> cards)
        {
            var straightFlushCardTypes = new List<CardType>();
            for (var i = 0; i < cardSuitCounts.Length; i++)
            {
                if (cardSuitCounts[i] >= ComparableCards)
                {
                    var cardTypeCounts = new int[15]; // Ace = 14
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
                        var bestStraight = new List<CardType>();
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

        private ICollection<CardType> GetFlushCards(ICollection<Card> cards)
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

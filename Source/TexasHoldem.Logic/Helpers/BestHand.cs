namespace TexasHoldem.Logic.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
            switch (this.RankType)
            {
                case HandRankType.HighCard:
                    return CompareTwoHandsWithHighCard(this.Cards, other.Cards);
                case HandRankType.Pair:
                    return CompareTwoHandsWithPair(this.Cards, other.Cards);
                case HandRankType.TwoPairs:
                    return CompareTwoHandsWithTwoPairs(this.Cards, other.Cards);
                case HandRankType.ThreeOfAKind:
                    return CompareTwoHandsWithThreeOfAKind(this.Cards, other.Cards);
                case HandRankType.Straight:
                    return CompareTwoHandsWithStraight(this.Cards, other.Cards);
                case HandRankType.Flush:
                    return CompareTwoHandsWithFlush(this.Cards, other.Cards);
                case HandRankType.FullHouse:
                    return CompareTwoHandsWithFullHouse(this.Cards, other.Cards);
                case HandRankType.FourOfAKind:
                    return CompareTwoHandsWithFourOfAKind(this.Cards, other.Cards);
                case HandRankType.StraightFlush:
                    return CompareTwoHandsWithStraightFlush(this.Cards, other.Cards);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static int CompareTwoHandsWithHighCard(
            ICollection<CardType> firstHand,
            ICollection<CardType> secondHand)
        {
            var firstSorted = firstHand.OrderBy(x => x).ToList();
            var secondSorted = secondHand.OrderBy(x => x).ToList();
            var cardsToCompare = Math.Min(firstHand.Count, secondHand.Count);
            for (var i = 0; i < cardsToCompare; i++)
            {
                if (firstSorted[i] > secondSorted[i])
                {
                    return 1;
                }

                if (firstSorted[i] < secondSorted[i])
                {
                    return -1;
                }
            }

            return 0;
        }

        private static int CompareTwoHandsWithPair(
            ICollection<CardType> firstHand,
            ICollection<CardType> secondHand)
        {
            var firstPairType = firstHand.GroupBy(x => x).First(x => x.Count() >= 2);
            var secondPairType = secondHand.GroupBy(x => x).First(x => x.Count() >= 2);

            if (firstPairType.Key > secondPairType.Key)
            {
                return 1;
            }

            if (firstPairType.Key < secondPairType.Key)
            {
                return -1;
            }

            // Equal pair => compare high card
            return CompareTwoHandsWithHighCard(firstHand, secondHand);
        }

        private static int CompareTwoHandsWithTwoPairs(
            ICollection<CardType> firstHand,
            ICollection<CardType> secondHand)
        {
            return 0;
        }

        private static int CompareTwoHandsWithThreeOfAKind(
            ICollection<CardType> firstHand,
            ICollection<CardType> secondHand)
        {
            return 0;
        }

        private static int CompareTwoHandsWithStraight(
            ICollection<CardType> firstHand,
            ICollection<CardType> secondHand)
        {
            return 0;
        }

        private static int CompareTwoHandsWithFlush(
            ICollection<CardType> firstHand,
            ICollection<CardType> secondHand)
        {
            return 0;
        }

        private static int CompareTwoHandsWithFullHouse(
            ICollection<CardType> firstHand,
            ICollection<CardType> secondHand)
        {
            return 0;
        }

        private static int CompareTwoHandsWithFourOfAKind(
            ICollection<CardType> firstHand,
            ICollection<CardType> secondHand)
        {
            return 0;
        }

        private static int CompareTwoHandsWithStraightFlush(
            ICollection<CardType> firstHand,
            ICollection<CardType> secondHand)
        {
            return 0;
        }
    }
}

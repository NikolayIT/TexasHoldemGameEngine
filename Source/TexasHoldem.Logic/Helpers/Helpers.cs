namespace TexasHoldem.Logic.Helpers
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;

    public static class Helpers
    {
        private static readonly IHandEvaluator HandEvaluator = new HandEvaluator();

        public static HandRankType GetHandRank(ICollection<Card> cards)
        {
            return HandEvaluator.GetBestHand(cards).RankType;
        }

        public static int CompareCards(IEnumerable<Card> firstPlayerCards, IEnumerable<Card> secondPlayerCards)
        {
            var firstPlayerBestHand = HandEvaluator.GetBestHand(firstPlayerCards);
            var secondPlayerBestHand = HandEvaluator.GetBestHand(secondPlayerCards);
            return firstPlayerBestHand.CompareTo(secondPlayerBestHand);
        }
    }
}

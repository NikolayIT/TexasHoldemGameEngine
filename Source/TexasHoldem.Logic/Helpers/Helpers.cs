namespace TexasHoldem.Logic.Helpers
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;

    public static class Helpers
    {
        public static HandRankType GetHandRank(ICollection<Card> cards)
        {
            // TODO: Share common object for less memory usage
            var handEvaluator = new HandEvaluator();
            return handEvaluator.GetBestHand(cards).RankType;
        }

        public static int CompareCards(ICollection<Card> firstPlayerCards, ICollection<Card> secondPlayerCards)
        {
            var handEvaluator = new HandEvaluator();
            var firstPlayerBestHand = handEvaluator.GetBestHand(firstPlayerCards);
            var secondPlayerBestHand = handEvaluator.GetBestHand(secondPlayerCards);
            return firstPlayerBestHand.CompareTo(secondPlayerBestHand);
        }
    }
}

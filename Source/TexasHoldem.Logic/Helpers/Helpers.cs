namespace TexasHoldem.Logic.Helpers
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;

    /// <summary>
    /// Class containing helper methods for evaluating and comparing player's hands.
    /// </summary>
    public static class Helpers
    {
        private static readonly IHandEvaluator HandEvaluator = new HandEvaluator();

        /// <summary>
        /// Finds the best possible hand given a player's cards and all revealed comunity cards.
        /// </summary>
        /// <param name="cards">A player's cards + all revealed comunity cards</param>
        /// <returns>Returns value of HandRankType. For example Straight, Flush, etc</returns>
        public static HandRankType GetHandRank(ICollection<Card> cards)
        {
            return HandEvaluator.GetBestHand(cards).RankType;
        }

        /// <summary>
        /// Compares the cards of two opponents to see which one can make the strongest hand with the community cards.
        /// At least 5 cards are needed. Can be used during and after the Flop round.
        /// </summary>
        /// <param name="firstPlayerCards">First player cards + all revealed comunity cards</param>
        /// <param name="secondPlayerCards">Second player cards + all revealed comunity cards</param>
        /// <returns>Comparison result as int</returns>
        public static int CompareCards(IEnumerable<Card> firstPlayerCards, IEnumerable<Card> secondPlayerCards)
        {
            var firstPlayerBestHand = HandEvaluator.GetBestHand(firstPlayerCards);
            var secondPlayerBestHand = HandEvaluator.GetBestHand(secondPlayerCards);
            return firstPlayerBestHand.CompareTo(secondPlayerBestHand);
        }
    }
}

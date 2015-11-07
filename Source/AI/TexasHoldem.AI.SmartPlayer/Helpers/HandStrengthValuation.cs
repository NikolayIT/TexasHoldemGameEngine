namespace TexasHoldem.AI.SmartPlayer.Helpers
{
    using TexasHoldem.Logic.Cards;

    public static class HandStrengthValuation
    {
		private const int MaxCardTypeValue = 14;

        private static readonly int[,] StartingHandRecommendationsSuited =
            {
                { 3, 3, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1 }, // A
                { 0, 3, 3, 3, 3, 2, 1, 1, 1, 1, 1, 1, 1 }, // K
                { 0, 0, 3, 3, 3, 2, 2, 0, 0, 0, 0, 0, 0 }, // Q
                { 0, 0, 0, 3, 3, 3, 2, 1, 0, 0, 0, 0, 0 }, // J
                { 0, 0, 0, 0, 3, 3, 2, 1, 0, 0, 0, 0, 0 }, // 10
                { 0, 0, 0, 0, 0, 3, 2, 1, 1, 0, 0, 0, 0 }, // 9
                { 0, 0, 0, 0, 0, 0, 3, 1, 1, 0, 0, 0, 0 }, // 8
                { 0, 0, 0, 0, 0, 0, 0, 3, 1, 1, 0, 0, 0 }, // 7
                { 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 0, 0, 0 }, // 6
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 0, 0 }, // 5
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 }, // 4
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 }, // 3
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 } // 2
            };

        private static readonly int[,] StartingHandRecommendationsUnsuited =
            {
                { 3, 3, 3, 3, 3, 1, 1, 1, 0, 0, 0, 0, 0 }, // A
                { 0, 3, 3, 3, 2, 1, 0, 0, 0, 0, 0, 0, 0 }, // K
                { 0, 0, 3, 2, 2, 1, 0, 0, 0, 0, 0, 0, 0 }, // Q
                { 0, 0, 0, 3, 2, 1, 1, 0, 0, 0, 0, 0, 0 }, // J
                { 0, 0, 0, 0, 3, 1, 1, 0, 0, 0, 0, 0, 0 }, // 10
                { 0, 0, 0, 0, 0, 3, 1, 1, 0, 0, 0, 0, 0 }, // 9
                { 0, 0, 0, 0, 0, 0, 3, 1, 0, 0, 0, 0, 0 }, // 8
                { 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0 }, // 7
                { 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0 }, // 6
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0, 0, 0 }, // 5
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 }, // 4
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 }, // 3
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 } // 2
            };

        // http://www.rakebackpros.net/texas-holdem-starting-hands/
        public static CardValuationType PreFlop(Card firstCard, Card secondCard)
        {
            var value = firstCard.Suit == secondCard.Suit
							? (firstCard.Type > secondCard.Type
								   ? StartingHandRecommendationsSuited[MaxCardTypeValue - (int)firstCard.Type, MaxCardTypeValue - (int)secondCard.Type]
								   : StartingHandRecommendationsSuited[MaxCardTypeValue - (int)secondCard.Type, MaxCardTypeValue - (int)firstCard.Type])
							: (firstCard.Type > secondCard.Type
								   ? StartingHandRecommendationsUnsuited[MaxCardTypeValue - (int)firstCard.Type, MaxCardTypeValue - (int)secondCard.Type]
								   : StartingHandRecommendationsUnsuited[MaxCardTypeValue - (int)secondCard.Type, MaxCardTypeValue - (int)firstCard.Type]);

            switch (value)
            {
                case 0:
                    return CardValuationType.Unplayable;
                case 1:
                    return CardValuationType.NotRecommended;
                case 2:
                    return CardValuationType.Risky;
                case 3:
                    return CardValuationType.Recommended;
                default:
                    return CardValuationType.Unplayable;
            }
        }
    }
}

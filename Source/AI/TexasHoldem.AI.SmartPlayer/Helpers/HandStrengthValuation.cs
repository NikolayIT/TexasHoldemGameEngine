namespace TexasHoldem.AI.SmartPlayer.Helpers
{
    using TexasHoldem.Logic.Cards;

    public static class HandStrengthValuation
    {
        private const int MaxCardTypeValue = 14;

        private static readonly int[,] StartingHandRecommendations =
            {
                { 3, 3, 3, 3, 3, 2, 2, 2, 2, 1, 1, 1, 1 }, // AA AKs AQs AJs ATs A9s A8s A7s A6s A5s A4s A3s A2s
                { 3, 3, 3, 3, 3, 2, 1, 1, 1, 1, 1, 1, 1 }, // AKo KK KQs KJs KTs K9s K8s K7s K6s K5s K4s K3s K2s
                { 3, 3, 3, 3, 3, 2, 2, 0, 0, 0, 0, 0, 0 }, // AQo KQo QQ QJs QTs Q9s Q8s Q7s Q6s Q5s Q4s Q3s Q2s
                { 3, 3, 2, 3, 3, 3, 2, 1, 0, 0, 0, 0, 0 }, // AJo KJo QJo JJ JTs J9s J8s J7s J6s J5s J4s J3s J2s
                { 3, 2, 2, 2, 3, 3, 2, 1, 0, 0, 0, 0, 0 }, // ATo KTo QTo JTo TT T9s T8s T7s T6s T5s T4s T3s T2s
                { 1, 1, 1, 1, 1, 3, 2, 1, 1, 0, 0, 0, 0 }, // A9o K9o Q9o J9o T9o 99 98s 97s 96s 95s 94s 93s 92s
                { 1, 0, 0, 1, 1, 1, 3, 1, 1, 0, 0, 0, 0 }, // A8o K8o Q8o J8o T8o 98o 88 87s 86s 85s 84s 83s 82s
                { 1, 0, 0, 0, 0, 1, 1, 3, 1, 1, 0, 0, 0 }, // A7o K7o Q7o J7o T7o 97o 87o 77 76s 75s 74s 73s 72s
                { 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 0, 0, 0 }, // A6o K6o Q6o J6o T6o 96o 86o 76o 66 65s 64s 63s 62s
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 1, 0, 0 }, // A5o K5o Q5o J5o T5o 95o 85o 75o 65o 55 54s 53s 52s
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 }, // A4o K4o Q4o J4o T4o 94o 84o 74o 64o 54o 44 43s 42s
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 }, // A3o K3o Q3o J3o T3o 93o 83o 73o 63o 53o 43o 33 32s
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, // A2o K2o Q2o J2o T2o 92o 82o 72o 62o 52o 42o 32o 22
            };

        // http://www.rakebackpros.net/texas-holdem-starting-hands/
        public static CardValuationType PreFlop(Card firstCard, Card secondCard)
        {
            var value = firstCard.Suit == secondCard.Suit
                          ? (firstCard.Type > secondCard.Type
                                 ? StartingHandRecommendations[MaxCardTypeValue - (int)firstCard.Type, MaxCardTypeValue - (int)secondCard.Type]
                                 : StartingHandRecommendations[MaxCardTypeValue - (int)secondCard.Type, MaxCardTypeValue - (int)firstCard.Type])
                          : (firstCard.Type > secondCard.Type
                                 ? StartingHandRecommendations[MaxCardTypeValue - (int)secondCard.Type, MaxCardTypeValue - (int)firstCard.Type]
                                 : StartingHandRecommendations[MaxCardTypeValue - (int)firstCard.Type, MaxCardTypeValue - (int)secondCard.Type]);

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

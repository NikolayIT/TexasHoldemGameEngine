namespace TexasHoldem.Tests.Helpers
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Helpers;

    public class HandEvaluatorTests
    {
        // TODO: Add tests for GetRankType()
        private static readonly object[] GetRankTypeCases =
            {
                new object[]
                    {
                        HandRankType.HighCard,
                        new List<Card>
                            {
                                new Card(CardSuit.Spade, CardType.Seven),
                                new Card(CardSuit.Heart, CardType.Six),
                                new Card(CardSuit.Spade, CardType.Five),
                                new Card(CardSuit.Club, CardType.Three),
                                new Card(CardSuit.Diamond, CardType.Two)
                            }
                    },
            };

        [Test, TestCaseSource(nameof(GetRankTypeCases))]
        public void GetRankTypeShouldWorkCorrectly(HandRankType expectedHandRankType, ICollection<Card> cards)
        {
            var handEvaluator = new HandEvaluator();
            var actualHandRankType = handEvaluator.GetRankType(cards);
            Assert.AreEqual(expectedHandRankType, actualHandRankType);
        }
    }
}

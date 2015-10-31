namespace TexasHoldem.Logic.Tests.Cards
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using TexasHoldem.Logic.Cards;

    [TestFixture]
    public class CardExtensionsTests
    {
        [Test]
        public void CardSuitToFriendlyStringShouldReturnDifferentValidValueForEachPossibleParameter()
        {
            var values = new HashSet<string>();
            foreach (CardSuit cardSuitValue in Enum.GetValues(typeof(CardSuit)))
            {
                var stringValue = cardSuitValue.ToFriendlyString();
                Assert.IsFalse(values.Contains(stringValue), $"Duplicate string value \"{stringValue}\" for card suit \"{cardSuitValue}\"");
                values.Add(stringValue);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CardSuitToFriendlyStringShouldThrowAnExceptionWhenCalledOnAnInvalidValue()
        {
            var cardSuits = Enum.GetValues(typeof(CardSuit));
            var cardSuit = cardSuits.OfType<CardSuit>().Max() + 1;
            cardSuit.ToFriendlyString();
        }

        [Test]
        public void CardTypeToFriendlyStringShouldReturnDifferentValidValueForEachPossibleParameter()
        {
            var values = new HashSet<string>();
            foreach (CardType cardTypeValue in Enum.GetValues(typeof(CardType)))
            {
                var stringValue = cardTypeValue.ToFriendlyString();
                Assert.IsFalse(values.Contains(stringValue), $"Duplicate string value \"{stringValue}\" for card suit \"{cardTypeValue}\"");
                values.Add(stringValue);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CardTypeToFriendlyStringShouldThrowAnExceptionWhenCalledOnAnInvalidValue()
        {
            var cardTypes = Enum.GetValues(typeof(CardType));
            var cardType = cardTypes.OfType<CardType>().Max() + 1;
            cardType.ToFriendlyString();
        }
    }
}

namespace TexasHoldem.Logic.Tests.Cards
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;

    using TexasHoldem.Logic.Cards;

    [TestFixture]
    public class CardTests
    {
        [Test]
        public void ConstructorShouldUpdatePropertyValues()
        {
            var card = new Card(CardSuit.Spade, CardType.Queen);
            Assert.AreEqual(CardSuit.Spade, card.Suit);
            Assert.AreEqual(CardType.Queen, card.Type);
        }

        [TestCase(true, CardSuit.Spade, CardType.Ace, CardSuit.Spade, CardType.Ace)]
        [TestCase(false, CardSuit.Heart, CardType.Jack, CardSuit.Heart, CardType.Queen)]
        [TestCase(false, CardSuit.Heart, CardType.King, CardSuit.Spade, CardType.King)]
        [TestCase(false, CardSuit.Heart, CardType.Nine, CardSuit.Spade, CardType.Ten)]
        public void EqualsShouldWorkCorrectly(
            bool expectedValue,
            CardSuit firstCardSuit,
            CardType firstCardType,
            CardSuit secondCardSuit,
            CardType secondCardType)
        {
            var firstCard = new Card(firstCardSuit, firstCardType);
            var secondCard = new Card(secondCardSuit, secondCardType);
            Assert.AreEqual(expectedValue, firstCard.Equals(secondCard));
            Assert.AreEqual(expectedValue, secondCard.Equals(firstCard));
        }

        [Test]
        public void EqualsShouldReturnFalseWhenGivenNullValue()
        {
            var card = new Card(CardSuit.Club, CardType.Nine);
            var areEqual = card.Equals(null);
            Assert.IsFalse(areEqual);
        }

        [Test]
        public void EqualsShouldReturnFalseWhenGivenNonCardObject()
        {
            var card = new Card(CardSuit.Club, CardType.Nine);

            // ReSharper disable once SuspiciousTypeConversion.Global
            var areEqual = card.Equals(new CardTests());
            Assert.IsFalse(areEqual);
        }

        [Test]
        public void GetHashCodeShouldReturnDifferentValidValueForEachCardCombination()
        {
            var values = new HashSet<int>();
            foreach (CardSuit cardSuitValue in Enum.GetValues(typeof(CardSuit)))
            {
                foreach (CardType cardTypeValue in Enum.GetValues(typeof(CardType)))
                {
                    var card = new Card(cardSuitValue, cardTypeValue);
                    var cardHashCode = card.GetHashCode();
                    Assert.IsFalse(
                        values.Contains(cardHashCode),
                        $"Duplicate hash code \"{cardHashCode}\" for card \"{card}\"");
                    values.Add(cardHashCode);
                }
            }
        }

        [Test]
        public void ToStringShouldReturnDifferentValidValueForEachCardCombination()
        {
            var values = new HashSet<string>();
            foreach (CardSuit cardSuitValue in Enum.GetValues(typeof(CardSuit)))
            {
                foreach (CardType cardTypeValue in Enum.GetValues(typeof(CardType)))
                {
                    var card = new Card(cardSuitValue, cardTypeValue);
                    var cardToString = card.ToString();
                    Assert.IsFalse(
                        values.Contains(cardToString),
                        $"Duplicate string value \"{cardToString}\" for card \"{card}\"");
                    values.Add(cardToString);
                }
            }
        }

        [Test]
        public void CloneShouldReturnDifferentReference()
        {
            var card = new Card(CardSuit.Diamond, CardType.Queen);
            var newCard = card.DeepClone();
            Assert.AreNotSame(card, newCard);
        }

        [Test]
        public void CloneShouldReturnObjectOfTypeCard()
        {
            var card = new Card(CardSuit.Diamond, CardType.Queen);
            var newCard = card.DeepClone();
            Assert.IsInstanceOf<Card>(newCard);
        }

        [Test]
        public void CloneShouldReturnEqualObjectWithEqualProperties()
        {
            var card = new Card(CardSuit.Club, CardType.Ace);
            var newCard = card.DeepClone();
            Assert.IsNotNull(newCard);
            Assert.IsTrue(card.Equals(newCard));
            Assert.AreEqual(card.Suit, newCard.Suit);
            Assert.AreEqual(card.Type, newCard.Type);
        }

        [Test]
        public void CloneShouldReturnObjectWithTheSameHashCode()
        {
            var card = new Card(CardSuit.Spade, CardType.Nine);
            var newCard = card.DeepClone();
            Assert.IsNotNull(newCard);
            Assert.AreEqual(card.GetHashCode(), newCard.GetHashCode());
        }

        [Test]
        public void FromHashCodeShouldCreateCardsWithTheGivenHashCode()
        {
            foreach (CardSuit cardSuitValue in Enum.GetValues(typeof(CardSuit)))
            {
                foreach (CardType cardTypeValue in Enum.GetValues(typeof(CardType)))
                {
                    var card = new Card(cardSuitValue, cardTypeValue);
                    var hashCode = card.GetHashCode();
                    var newCard = Card.FromHashCode(hashCode);
                    Assert.AreEqual(card, newCard);
                }
            }
        }

        [TestCase(0, CardSuit.Club, CardType.Two)]
        [TestCase(1, CardSuit.Club, CardType.Three)]
        [TestCase(11, CardSuit.Club, CardType.King)]
        [TestCase(12, CardSuit.Club, CardType.Ace)]
        [TestCase(13, CardSuit.Diamond, CardType.Two)]
        [TestCase(24, CardSuit.Diamond, CardType.King)]
        [TestCase(25, CardSuit.Diamond, CardType.Ace)]
        [TestCase(26, CardSuit.Heart, CardType.Two)]
        [TestCase(27, CardSuit.Heart, CardType.Three)]
        [TestCase(37, CardSuit.Heart, CardType.King)]
        [TestCase(38, CardSuit.Heart, CardType.Ace)]
        [TestCase(39, CardSuit.Spade, CardType.Two)]
        [TestCase(44, CardSuit.Spade, CardType.Seven)]
        [TestCase(50, CardSuit.Spade, CardType.King)]
        [TestCase(51, CardSuit.Spade, CardType.Ace)]
        public void GetHashCodeShouldReturn52ForKingOfSpades(int expectedHashCode, CardSuit cardSuit, CardType cardType)
        {
            var card = new Card(cardSuit, cardType);
            var hashCode = card.GetHashCode();
            Assert.AreEqual(expectedHashCode, hashCode);
        }
    }
}

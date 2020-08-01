namespace TexasHoldem.Logic.Tests.Cards
{
    using System;
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;

    using Xunit;

    public class CardTests
    {
        [Fact]
        public void ConstructorShouldUpdatePropertyValues()
        {
            var card = new Card(CardSuit.Spade, CardType.Queen);
            Assert.Equal(CardSuit.Spade, card.Suit);
            Assert.Equal(CardType.Queen, card.Type);
        }

        [Theory]
        [InlineData(true, CardSuit.Spade, CardType.Ace, CardSuit.Spade, CardType.Ace)]
        [InlineData(false, CardSuit.Heart, CardType.Jack, CardSuit.Heart, CardType.Queen)]
        [InlineData(false, CardSuit.Heart, CardType.King, CardSuit.Spade, CardType.King)]
        [InlineData(false, CardSuit.Heart, CardType.Nine, CardSuit.Spade, CardType.Ten)]
        public void EqualsShouldWorkCorrectly(
            bool expectedValue,
            CardSuit firstCardSuit,
            CardType firstCardType,
            CardSuit secondCardSuit,
            CardType secondCardType)
        {
            var firstCard = new Card(firstCardSuit, firstCardType);
            var secondCard = new Card(secondCardSuit, secondCardType);
            Assert.Equal(expectedValue, firstCard.Equals(secondCard));
            Assert.Equal(expectedValue, secondCard.Equals(firstCard));
        }

        [Fact]
        public void EqualsShouldReturnFalseWhenGivenNullValue()
        {
            var card = new Card(CardSuit.Club, CardType.Nine);
            var areEqual = card.Equals(null);
            Assert.False(areEqual);
        }

        [Fact]
        public void EqualsShouldReturnFalseWhenGivenNonCardObject()
        {
            var card = new Card(CardSuit.Club, CardType.Nine);

            // ReSharper disable once SuspiciousTypeConversion.Global
            var areEqual = card.Equals(new CardTests());
            Assert.False(areEqual);
        }

        [Fact]
        public void GetHashCodeShouldReturnDifferentValidValueForEachCardCombination()
        {
            var values = new HashSet<int>();
            foreach (CardSuit cardSuitValue in Enum.GetValues(typeof(CardSuit)))
            {
                foreach (CardType cardTypeValue in Enum.GetValues(typeof(CardType)))
                {
                    var card = new Card(cardSuitValue, cardTypeValue);
                    var cardHashCode = card.GetHashCode();
                    Assert.False(
                        values.Contains(cardHashCode),
                        $"Duplicate hash code \"{cardHashCode}\" for card \"{card}\"");
                    values.Add(cardHashCode);
                }
            }
        }

        [Fact]
        public void ToStringShouldReturnDifferentValidValueForEachCardCombination()
        {
            var values = new HashSet<string>();
            foreach (CardSuit cardSuitValue in Enum.GetValues(typeof(CardSuit)))
            {
                foreach (CardType cardTypeValue in Enum.GetValues(typeof(CardType)))
                {
                    var card = new Card(cardSuitValue, cardTypeValue);
                    var cardToString = card.ToString();
                    Assert.False(
                        values.Contains(cardToString),
                        $"Duplicate string value \"{cardToString}\" for card \"{card}\"");
                    values.Add(cardToString);
                }
            }
        }

        [Fact]
        public void CloneShouldReturnDifferentReference()
        {
            var card = new Card(CardSuit.Diamond, CardType.Queen);
            var newCard = card.DeepClone();
            Assert.NotSame(card, newCard);
        }

        [Fact]
        public void CloneShouldReturnObjectOfTypeCard()
        {
            var card = new Card(CardSuit.Diamond, CardType.Queen);
            var newCard = card.DeepClone();
            Assert.IsType<Card>(newCard);
        }

        [Fact]
        public void CloneShouldReturnEqualObjectWithEqualProperties()
        {
            var card = new Card(CardSuit.Club, CardType.Ace);
            var newCard = card.DeepClone();
            Assert.NotNull(newCard);
            Assert.True(card.Equals(newCard));
            Assert.Equal(card.Suit, newCard.Suit);
            Assert.Equal(card.Type, newCard.Type);
        }

        [Fact]
        public void CloneShouldReturnObjectWithTheSameHashCode()
        {
            var card = new Card(CardSuit.Spade, CardType.Nine);
            var newCard = card.DeepClone();
            Assert.NotNull(newCard);
            Assert.Equal(card.GetHashCode(), newCard.GetHashCode());
        }

        [Fact]
        public void FromHashCodeShouldCreateCardsWithTheGivenHashCode()
        {
            foreach (CardSuit cardSuitValue in Enum.GetValues(typeof(CardSuit)))
            {
                foreach (CardType cardTypeValue in Enum.GetValues(typeof(CardType)))
                {
                    var card = new Card(cardSuitValue, cardTypeValue);
                    var hashCode = card.GetHashCode();
                    var newCard = Card.FromHashCode(hashCode);
                    Assert.Equal(card, newCard);
                }
            }
        }

        [Theory]
        [InlineData(0, CardSuit.Club, CardType.Two)]
        [InlineData(1, CardSuit.Club, CardType.Three)]
        [InlineData(11, CardSuit.Club, CardType.King)]
        [InlineData(12, CardSuit.Club, CardType.Ace)]
        [InlineData(13, CardSuit.Diamond, CardType.Two)]
        [InlineData(24, CardSuit.Diamond, CardType.King)]
        [InlineData(25, CardSuit.Diamond, CardType.Ace)]
        [InlineData(26, CardSuit.Heart, CardType.Two)]
        [InlineData(27, CardSuit.Heart, CardType.Three)]
        [InlineData(37, CardSuit.Heart, CardType.King)]
        [InlineData(38, CardSuit.Heart, CardType.Ace)]
        [InlineData(39, CardSuit.Spade, CardType.Two)]
        [InlineData(44, CardSuit.Spade, CardType.Seven)]
        [InlineData(50, CardSuit.Spade, CardType.King)]
        [InlineData(51, CardSuit.Spade, CardType.Ace)]
        public void GetHashCodeShouldReturn52ForKingOfSpades(int expectedHashCode, CardSuit cardSuit, CardType cardType)
        {
            var card = new Card(cardSuit, cardType);
            var hashCode = card.GetHashCode();
            Assert.Equal(expectedHashCode, hashCode);
        }
    }
}

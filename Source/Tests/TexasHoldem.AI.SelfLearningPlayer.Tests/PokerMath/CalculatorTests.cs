namespace TexasHoldem.AI.SelfLearningPlayer.Tests.PokerMath
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;
    using TexasHoldem.AI.SelfLearningPlayer.PokerMath;
    using TexasHoldem.Logic.Cards;

    [TestFixture]
    public class CalculatorTests
    {
        [Test]
        public void ConstructorShouldThrowArgumentNullExceptionWhenHeroHoleCardsIsIncorrect()
        {
            Assert.Throws<ArgumentNullException>(() => { new Calculator(null, null, null); });
        }

        [Test]
        public void ConstructorShouldThrowArgumentOutOfRangeExceptionWhenHeroHoleCardsContainsOnlyOneCard()
        {
            var heroHoleCards = new List<Card> { new Card(CardSuit.Heart, CardType.Jack) };
            Assert.Throws<ArgumentOutOfRangeException>(() => { new Calculator(heroHoleCards, null, null); });
        }

        [Test]
        public void ConstructorShouldThrowArgumentNullExceptionWhenOpponentsHoleCardsIsIncorrect()
        {
            var heroHoleCards = new List<Card> { new Card(CardSuit.Spade, CardType.Five), new Card(CardSuit.Diamond, CardType.Nine) };
            Assert.Throws<ArgumentNullException>(() => { new Calculator(heroHoleCards, null, null); });
        }

        [Test]
        public void ConstructorShouldThrowArgumentOutOfRangeExceptionWhenOpponentsHoleCardsContainsOnlyOneCard()
        {
            var heroHoleCards = new List<Card> { new Card(CardSuit.Club, CardType.Six), new Card(CardSuit.Spade, CardType.King) };
            var opponentsHoleCards = new List<ICollection<Card>>();
            opponentsHoleCards.Add(new List<Card> { new Card(CardSuit.Diamond, CardType.Three) });
            Assert.Throws<ArgumentOutOfRangeException>(() => { new Calculator(heroHoleCards, opponentsHoleCards, null); });
        }

        [Test]
        public void ConstructorShouldThrowArgumentNullExceptionWhenCommunityCardsIsIncorrect()
        {
            var heroHoleCards = new List<Card> { new Card(CardSuit.Club, CardType.Six), new Card(CardSuit.Spade, CardType.King) };
            var opponentsHoleCards = new List<ICollection<Card>>();
            opponentsHoleCards.Add(new List<Card> { new Card(CardSuit.Diamond, CardType.Three), new Card(CardSuit.Heart, CardType.Queen) });
            Assert.Throws<ArgumentNullException>(() => { new Calculator(heroHoleCards, opponentsHoleCards, null); });
        }

        [Test]
        public void EquityShouldReturnCorrectValue()
        {
            /*
             * PokerStove
             *            equity     win       tie       pots won   pots tied
             *  AsKc:     30.274%    29.63%    00.64%    243        5.25
             *  Qh3d:     36.128%    35.49%    00.64%    291        5.25
             *  9s2c:     19.421%    18.78%    00.64%    154        5.25
             *  JhTd:     14.177%    13.54%    00.64%    111        5.25
            */

            var heroHoleCards = new List<Card> { new Card(CardSuit.Club, CardType.King), new Card(CardSuit.Spade, CardType.Ace) };
            var opponentsHoleCards = new List<ICollection<Card>>();
            opponentsHoleCards.Add(new List<Card> { new Card(CardSuit.Diamond, CardType.Three), new Card(CardSuit.Heart, CardType.Queen) });
            opponentsHoleCards.Add(new List<Card> { new Card(CardSuit.Club, CardType.Two), new Card(CardSuit.Spade, CardType.Nine) });
            opponentsHoleCards.Add(new List<Card> { new Card(CardSuit.Heart, CardType.Jack), new Card(CardSuit.Diamond, CardType.Ten) });
            var flop = new List<Card> { new Card(CardSuit.Diamond, CardType.Four), new Card(CardSuit.Heart, CardType.Five), new Card(CardSuit.Club, CardType.Six) };
            var calc = new Calculator(heroHoleCards, opponentsHoleCards, flop);
            Assert.AreEqual(0.30274, calc.Equity(), 0.00001);
        }
    }
}

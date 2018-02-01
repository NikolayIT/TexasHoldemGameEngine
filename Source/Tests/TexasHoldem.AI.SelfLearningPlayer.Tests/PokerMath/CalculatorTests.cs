namespace TexasHoldem.AI.SelfLearningPlayer.Tests.PokerMath
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Moq;
    using NUnit.Framework;
    using TexasHoldem.AI.SelfLearningPlayer.PokerMath;
    using TexasHoldem.Logic.Cards;

    [TestFixture]
    public class CalculatorTests
    {
        [Test]
        public void ConstructorShouldThrowArgumentNullExceptionWhenPocketIsIncorrect()
        {
            Assert.Throws<ArgumentNullException>(() => { new Calculator(null, null, null); });
        }

        [Test]
        public void ConstructorShouldThrowArgumentOutOfRangeExceptionWhenPocketsAreLessThanTwo()
        {
            var mockedPocket = new Mock<IPocket>();
            var pockets = new List<IPocket> { mockedPocket.Object };
            Assert.Throws<ArgumentOutOfRangeException>(() => { new Calculator(pockets, null, null); });
        }

        [Test]
        public void ConstructorShouldThrowArgumentNullExceptionWhenDeadCardsIsIncorrect()
        {
            var mockedPocket = new Mock<IPocket>();
            var pockets = new List<IPocket> { mockedPocket.Object, mockedPocket.Object };
            Assert.Throws<ArgumentNullException>(() => { new Calculator(pockets, null, null); });
        }

        [Test]
        public void ConstructorShouldThrowArgumentNullExceptionWhenCommunityCardsIsIncorrect()
        {
            var mockedPocket = new Mock<IPocket>();
            var pockets = new List<IPocket> { mockedPocket.Object, mockedPocket.Object };
            Assert.Throws<ArgumentNullException>(() => { new Calculator(pockets, new List<Card>(), null); });
        }

        [Test]
        public void EquityShouldReturnCorrectValue()
        {
            /*
             * PokerStove:
             * Board: 4d 5h 6c
             * Dead:  7h Ad
             *            equity       win         tie         pots won      pots tied
             * Hand 0:    29.352%      28.74%      00.61%      213           4.50   { AsKc }
             * Hand 1:    34.615%      34.01%      00.61%      252           4.50   { Qh3d }
             * Hand 2:    20.445%      19.84%      00.61%      147           4.50   { 9s2c }
             * Hand 3:    15.587%      14.98%      00.61%      111           4.50   { JhTd }
             *
            */

            var pockets = new List<IPocket>();
            pockets.Add(new Pocket(new List<Card> { new Card(CardSuit.Club, CardType.King), new Card(CardSuit.Spade, CardType.Ace) }));
            pockets.Add(new Pocket(new List<Card> { new Card(CardSuit.Diamond, CardType.Three), new Card(CardSuit.Heart, CardType.Queen) }));
            pockets.Add(new Pocket(new List<Card> { new Card(CardSuit.Club, CardType.Two), new Card(CardSuit.Spade, CardType.Nine) }));
            pockets.Add(new Pocket(new List<Card> { new Card(CardSuit.Heart, CardType.Jack), new Card(CardSuit.Diamond, CardType.Ten) }));
            var flop = new List<Card> { new Card(CardSuit.Diamond, CardType.Four), new Card(CardSuit.Heart, CardType.Five), new Card(CardSuit.Club, CardType.Six) };
            var calc = new Calculator(pockets, new Card[] { new Card(CardSuit.Heart, CardType.Seven), new Card(CardSuit.Diamond, CardType.Ace) }, flop);

            var list = calc.Equity().ToList();

            Assert.AreEqual(4, list.Count);

            Assert.AreEqual("As Kc", list[0].Pocket.Text);
            Assert.AreEqual(0.29352, list[0].Equity, 0.00001);

            Assert.AreEqual("Qh 3d", list[1].Pocket.Text);
            Assert.AreEqual(0.34615, list[1].Equity, 0.00001);

            Assert.AreEqual("9s 2c", list[2].Pocket.Text);
            Assert.AreEqual(0.20445, list[2].Equity, 0.00001);

            Assert.AreEqual("Jh Td", list[3].Pocket.Text);
            Assert.AreEqual(0.15587, list[3].Equity, 0.00001);
        }
    }
}

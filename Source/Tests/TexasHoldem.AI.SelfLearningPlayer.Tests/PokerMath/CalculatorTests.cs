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
        public void ConstructorShouldThrowArgumentNullExceptionWhenPocketsIsIncorrect()
        {
            Assert.Throws<ArgumentNullException>(() => { new Calculator(null, null); });
        }

        [Test]
        public void ConstructorShouldThrowArgumentOutOfRangeExceptionWhenLessThanTwoPockets()
        {
            var mockedPocket = new Mock<IPocket>();
            var pockets = new List<IPocket> { mockedPocket.Object };
            Assert.Throws<ArgumentOutOfRangeException>(() => { new Calculator(pockets, null); });
        }

        [Test]
        public void ConstructorShouldThrowArgumentNullExceptionWhenCommunityCardsIsIncorrect()
        {
            var mockedPocket = new Mock<IPocket>();
            var pockets = new List<IPocket> { mockedPocket.Object, mockedPocket.Object };
            Assert.Throws<ArgumentNullException>(() => { new Calculator(pockets, null); });
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

            var pockets = new List<IPocket>();
            pockets.Add(new Pocket(new List<Card> { new Card(CardSuit.Club, CardType.King), new Card(CardSuit.Spade, CardType.Ace) }));
            pockets.Add(new Pocket(new List<Card> { new Card(CardSuit.Diamond, CardType.Three), new Card(CardSuit.Heart, CardType.Queen) }));
            pockets.Add(new Pocket(new List<Card> { new Card(CardSuit.Club, CardType.Two), new Card(CardSuit.Spade, CardType.Nine) }));
            pockets.Add(new Pocket(new List<Card> { new Card(CardSuit.Heart, CardType.Jack), new Card(CardSuit.Diamond, CardType.Ten) }));
            var flop = new List<Card> { new Card(CardSuit.Diamond, CardType.Four), new Card(CardSuit.Heart, CardType.Five), new Card(CardSuit.Club, CardType.Six) };
            var calc = new Calculator(pockets, flop);

            var list = calc.Equity().ToList();

            Assert.AreEqual(4, list.Count);

            Assert.AreEqual("As Kc", list[0].Pocket.Text);
            Assert.AreEqual(0.30274, list[0].Equity, 0.00001);

            Assert.AreEqual("Qh 3d", list[1].Pocket.Text);
            Assert.AreEqual(0.36128, list[1].Equity, 0.00001);

            Assert.AreEqual("9s 2c", list[2].Pocket.Text);
            Assert.AreEqual(0.19420, list[2].Equity, 0.00001);

            Assert.AreEqual("Jh Td", list[3].Pocket.Text);
            Assert.AreEqual(0.14176, list[3].Equity, 0.00001);
        }
    }
}

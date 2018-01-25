namespace TexasHoldem.AI.SelfLearningPlayer.Tests.PokerMath
{
    using System;
    using System.Collections.Generic;

    using NUnit.Framework;
    using TexasHoldem.AI.SelfLearningPlayer.PokerMath;
    using TexasHoldem.Logic.Cards;

    [TestFixture]
    public class CardAdapterTests
    {
        [Test]
        public void ConstructorShouldThrowArgumentNullExceptionWhenAnIncorrectCollection()
        {
            Assert.Throws<ArgumentNullException>(() => { new CardAdapter(null); });
        }

        [Test]
        public void ToStingShouldReturnTwoCardsOfSpecificFormat()
        {
            var list = new List<Card> { new Card(CardSuit.Club, CardType.Ace), new Card(CardSuit.Diamond, CardType.Ten) };
            var adapter = new CardAdapter(list);
            Assert.AreEqual("AcTd", adapter.ToString());
        }

        [Test]
        public void MaskShouldReturnNonzeroValue()
        {
            var list = new List<Card> { new Card(CardSuit.Spade, CardType.Two) };
            var adapter = new CardAdapter(list);
            Assert.AreNotEqual(0, adapter.Mask);
        }
    }
}

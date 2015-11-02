namespace TexasHoldem.Logic.Tests.Cards
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Cards;

    [TestFixture]
    public class DeckTests
    {
        [Test]
        public void GetNextCardShouldNotThrowExceptionWhenCalled52Times()
        {
            IDeck deck = new Deck();
            for (var i = 0; i < 52; i++)
            {
                deck.GetNextCard();
            }
        }

        [Test]
        public void GetNextCardShouldReturnAll52CardsOnce()
        {
            IDeck deck = new Deck();
            var cards = new List<Card>();

            for (var i = 0; i < 52; i++)
            {
                cards.Add(deck.GetNextCard());
            }

            CollectionAssert.AreEquivalent(Deck.AllCards, cards);
        }

        [Test]
        [ExpectedException(typeof(InternalGameException))]
        public void GetNextCardShouldThrowExceptionWhenCalled53Times()
        {
            IDeck deck = new Deck();
            for (var i = 0; i < 53; i++)
            {
                deck.GetNextCard();
            }
        }
    }
}

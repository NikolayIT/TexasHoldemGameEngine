namespace TexasHoldem.Logic.Tests.Cards
{
    using System.Collections.Generic;

    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Cards;

    using Xunit;

    public class DeckTests
    {
        [Fact]
        public void GetNextCardShouldNotThrowExceptionWhenCalled52Times()
        {
            IDeck deck = new Deck();
            for (var i = 0; i < 52; i++)
            {
                deck.GetNextCard();
            }
        }

        [Fact]
        public void GetNextCardShouldReturnAll52CardsOnce()
        {
            IDeck deck = new Deck();
            var cards = new List<Card>();

            for (var i = 0; i < 52; i++)
            {
                cards.Add(deck.GetNextCard());
            }

            CollectionsAssert.SameElements(Deck.AllCards, cards);
        }

        [Fact]
        public void GetNextCardShouldThrowExceptionWhenCalled53Times()
        {
            IDeck deck = new Deck();
            for (var i = 0; i < 53; i++)
            {
                if (i < 52)
                {
                    deck.GetNextCard();
                }
                else
                {
                    Assert.Throws<InternalGameException>(() => deck.GetNextCard());
                }
            }
        }
    }
}

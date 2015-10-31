namespace TexasHoldem.Logic.Tests.Cards
{
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

namespace TexasHoldem.Logic.Helpers
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;

    public interface IHandEvaluator
    {
        BestHand GetBestHand(ICollection<Card> cards);
    }
}
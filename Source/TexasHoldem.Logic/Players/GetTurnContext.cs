namespace TexasHoldem.Logic.Players
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;

    public class GetTurnContext
    {
        public GetTurnContext(ICollection<Card> communityCards)
        {
            this.CommunityCards = communityCards;
        }

        public ICollection<Card> CommunityCards { get; }
    }
}

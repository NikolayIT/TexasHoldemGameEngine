namespace TexasHoldem.Logic.Players
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;

    public class GetTurnContext
    {
        public GetTurnContext(ICollection<Card> communityCards, int potBeforeThisRound, ICollection<PlayerActionAndName> bets)
        {
            this.CommunityCards = communityCards;
            this.PotBeforeThisRound = potBeforeThisRound;
            this.Bets = bets;
        }

        public ICollection<Card> CommunityCards { get; }

        public int PotBeforeThisRound { get; set; }

        public ICollection<PlayerActionAndName> Bets { get; set; }
    }
}

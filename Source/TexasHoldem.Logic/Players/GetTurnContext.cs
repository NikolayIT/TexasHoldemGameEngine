namespace TexasHoldem.Logic.Players
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;

    public class GetTurnContext
    {
        public GetTurnContext(
            ICollection<Card> communityCards,
            GameRoundType roundType,
            int potBeforeTheRound,
            ICollection<PlayerActionAndName> bets,
            int currentPot)
        {
            this.CommunityCards = communityCards;
            this.RoundType = roundType;
            this.PotBeforeTheRound = potBeforeTheRound;
            this.Bets = bets;
            this.CurrentPot = currentPot;
        }

        public ICollection<Card> CommunityCards { get; }

        public GameRoundType RoundType { get; }

        public int PotBeforeTheRound { get; }

        public ICollection<PlayerActionAndName> Bets { get; }

        public int CurrentPot { get; }
    }
}

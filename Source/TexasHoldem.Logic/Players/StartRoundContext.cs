namespace TexasHoldem.Logic.Players
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;

    public class StartRoundContext
    {
        public StartRoundContext(GameRoundType roundType, IReadOnlyCollection<Card> communityCards, int currentPot)
        {
            this.RoundType = roundType;
            this.CommunityCards = communityCards;
            this.CurrentPot = currentPot;
        }

        public GameRoundType RoundType { get; }

        public IReadOnlyCollection<Card> CommunityCards { get; }

        public int CurrentPot { get; set; }
    }
}

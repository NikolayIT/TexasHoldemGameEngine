namespace TexasHoldem.Logic.Players
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;

    public class StartRoundContext : IStartRoundContext
    {
        public StartRoundContext(
            GameRoundType roundType,
            IReadOnlyCollection<Card> communityCards,
            int moneyLeft,
            int currentPot,
            IDictionary<string, IReadOnlyCollection<Card>> holeCardsOfOpponentsWhoAreInHand)
        {
            this.RoundType = roundType;
            this.CommunityCards = communityCards;
            this.MoneyLeft = moneyLeft;
            this.CurrentPot = currentPot;
            this.HoleCardsOfOpponentsWhoAreInHand = holeCardsOfOpponentsWhoAreInHand;
        }

        public GameRoundType RoundType { get; }

        public IReadOnlyCollection<Card> CommunityCards { get; }

        public int MoneyLeft { get; }

        public int CurrentPot { get; }

        public IDictionary<string, IReadOnlyCollection<Card>> HoleCardsOfOpponentsWhoAreInHand { get; }
    }
}

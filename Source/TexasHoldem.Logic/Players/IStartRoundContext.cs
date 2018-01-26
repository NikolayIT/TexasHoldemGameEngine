using System.Collections.Generic;
using TexasHoldem.Logic.Cards;

namespace TexasHoldem.Logic.Players
{
    public interface IStartRoundContext
    {
        IReadOnlyCollection<Card> CommunityCards { get; }
        int CurrentPot { get; }
        int MoneyLeft { get; }
        GameRoundType RoundType { get; }
        IDictionary<string, IReadOnlyCollection<Card>> HoleCardsOfOpponentsWhoAreInHand { get; }
    }
}
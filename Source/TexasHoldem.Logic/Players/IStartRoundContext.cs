using System.Collections.Generic;
using TexasHoldem.Logic.Cards;
using TexasHoldem.Logic.GameMechanics;

namespace TexasHoldem.Logic.Players
{
    public interface IStartRoundContext
    {
        IReadOnlyCollection<Card> CommunityCards { get; }
        int CurrentPot { get; }
        int MoneyLeft { get; }
        GameRoundType RoundType { get; }
        Pot CurrentMainPot { get; }
        List<Pot> CurrentSidePots { get; }
    }
}
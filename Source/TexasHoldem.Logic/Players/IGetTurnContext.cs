using System.Collections.Generic;

namespace TexasHoldem.Logic.Players
{
    public interface IGetTurnContext
    {
        bool CanCheck { get; }
        int CurrentMaxBet { get; }
        int CurrentPot { get; }
        bool IsAllIn { get; }
        int MoneyLeft { get; }
        int MoneyToCall { get; }
        int MyMoneyInTheRound { get; }
        IReadOnlyCollection<PlayerActionAndName> PreviousRoundActions { get; }
        GameRoundType RoundType { get; }
        int SmallBlind { get; }
        int MinRaise { get; }
    }
}
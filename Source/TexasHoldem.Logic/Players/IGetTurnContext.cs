namespace TexasHoldem.Logic.Players
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.GameMechanics;

    public interface IGetTurnContext
    {
        bool CanCheck { get; }

        bool CanRaise { get; }

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

        Pot MainPot { get; }

        IReadOnlyCollection<Pot> SidePots { get; }
    }
}

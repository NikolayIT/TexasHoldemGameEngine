namespace TexasHoldem.AI.DummyPlayer
{
    using System;

    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Players;

    public class DummyPlayer : BasePlayer
    {
        public override string Name { get; } = "DummyPlayer_" + Guid.NewGuid();

        public override int BuyIn { get; } = -1;

        public override PlayerAction PostingBlind(IPostingBlindContext context)
        {
            return context.BlindAction;
        }

        public override PlayerAction GetTurn(IGetTurnContext context)
        {
            var chanceForAction = RandomProvider.Next(1, 101);
            if (chanceForAction == 1 && context.MoneyLeft > 0)
            {
                // All-in
                return PlayerAction.Raise(context.MoneyLeft - context.MoneyToCall);
            }

            if (chanceForAction <= 15)
            {
                if (context.AvailablePlayerOptions.Contains(PlayerActionType.Raise))
                {
                    if (context.MinRaise + context.CurrentMaxBet > context.MoneyLeft)
                    {
                        // All-in
                        return PlayerAction.Raise(context.MoneyLeft - context.MoneyToCall);
                    }
                    else
                    {
                        // Minimum raise
                        return PlayerAction.Raise(context.MinRaise);
                    }
                }
                else
                {
                    return PlayerAction.CheckOrCall();
                }
            }

            // Play safe
            if (context.CanCheck)
            {
                return PlayerAction.CheckOrCall();
            }

            if (chanceForAction <= 60)
            {
                // Call
                return PlayerAction.CheckOrCall();
            }
            else
            {
                // Fold
                return PlayerAction.Fold();
            }
        }
    }
}

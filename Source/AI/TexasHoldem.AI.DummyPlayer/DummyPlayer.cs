namespace TexasHoldem.AI.DummyPlayer
{
    using System;

    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Players;

    public class DummyPlayer : BasePlayer
    {
        public override string Name { get; } = "DummyPlayer_" + Guid.NewGuid();

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            var chanceForAction = RandomProvider.Next(1, 101);
            if (chanceForAction == 1 && context.MoneyLeft > 0)
            {
                // All-in
                return PlayerAction.Raise(context.MoneyLeft);
            }

            if (chanceForAction <= 15)
            {
                // Minimum raise
                return PlayerAction.Raise(1);
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

namespace TexasHoldem.AI.SmartPlayer
{
    using System;

    using TexasHoldem.Logic.Players;

    public class SmartPlayer : BasePlayer
    {
        public override string Name { get; } = "SmartPlayer_" + Guid.NewGuid();

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            // TODO: Implement smart logic
            return PlayerAction.Fold();
        }
    }
}

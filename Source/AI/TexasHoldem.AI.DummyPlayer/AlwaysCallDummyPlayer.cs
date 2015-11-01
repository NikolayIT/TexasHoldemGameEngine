namespace TexasHoldem.AI.DummyPlayer
{
    using System;

    using TexasHoldem.Logic.Players;

    internal class AlwaysCallDummyPlayer : BasePlayer
    {
        public override string Name { get; } = "AlwaysCallDummyPlayer_" + Guid.NewGuid();

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            return PlayerAction.CheckOrCall();
        }
    }
}

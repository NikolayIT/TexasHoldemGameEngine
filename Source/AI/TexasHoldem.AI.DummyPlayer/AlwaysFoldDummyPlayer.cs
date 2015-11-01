namespace TexasHoldem.AI.DummyPlayer
{
    using System;

    using TexasHoldem.Logic.Players;

    internal class AlwaysFoldDummyPlayer : BasePlayer
    {
        public override string Name { get; } = "AlwaysFoldDummyPlayer_" + Guid.NewGuid();

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            return PlayerAction.Fold();
        }
    }
}

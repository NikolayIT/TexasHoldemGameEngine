namespace TexasHoldem.AI.DummyPlayer
{
    using System;

    using TexasHoldem.Logic.Players;

    internal class AlwaysRaiseDummyPlayer : BasePlayer
    {
        public override string Name { get; } = "AlwaysRaiseDummyPlayer_" + Guid.NewGuid();

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            return PlayerAction.Raise(context.SmallBlind);
        }
    }
}

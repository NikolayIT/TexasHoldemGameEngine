namespace TexasHoldem.AI.DummyPlayer
{
    using System;

    using TexasHoldem.Logic.Players;

    public class DummyPlayer : BasePlayer
    {
        public override string Name { get; } = "DummyPlayer_" + Guid.NewGuid();

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            // TODO: Raise/Call(Check)/Fold on random
            return PlayerAction.Fold();
        }
    }
}

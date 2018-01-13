namespace TexasHoldem.AI.DummyPlayer
{
    using System;

    using TexasHoldem.Logic.Players;

    internal class AlwaysCallDummyPlayer : BasePlayer
    {
        public override string Name { get; } = "AlwaysCallDummyPlayer_" + Guid.NewGuid();

        public override PlayerAction ToPostBlind(int stackSize, int blindSize, int currentPot)
        {
            throw new NotImplementedException();
        }

        public override PlayerAction GetTurn(IGetTurnContext context)
        {
            return PlayerAction.CheckOrCall();
        }
    }
}

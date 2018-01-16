namespace TexasHoldem.AI.DummyPlayer
{
    using System;

    using TexasHoldem.Logic.Players;

    internal class AlwaysCallDummyPlayer : BasePlayer
    {
        public override string Name { get; } = "AlwaysCallDummyPlayer_" + Guid.NewGuid();

        public override int BuyIn { get; } = -1;

        public override PlayerAction PostingBlind(IPostingBlindContext context)
        {
            throw new NotImplementedException();
        }

        public override PlayerAction GetTurn(IGetTurnContext context)
        {
            return PlayerAction.CheckOrCall();
        }
    }
}

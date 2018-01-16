namespace TexasHoldem.AI.DummyPlayer
{
    using System;

    using TexasHoldem.Logic.Players;

    internal class AlwaysFoldDummyPlayer : BasePlayer
    {
        public override string Name { get; } = "AlwaysFoldDummyPlayer_" + Guid.NewGuid();

        public override int BuyIn { get; } = -1;

        public override PlayerAction PostingBlind(IPostingBlindContext context)
        {
            throw new NotImplementedException();
        }

        public override PlayerAction GetTurn(IGetTurnContext context)
        {
            return PlayerAction.Fold();
        }
    }
}

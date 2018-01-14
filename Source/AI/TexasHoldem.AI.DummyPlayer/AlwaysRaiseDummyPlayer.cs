namespace TexasHoldem.AI.DummyPlayer
{
    using System;

    using TexasHoldem.Logic.Players;

    internal class AlwaysRaiseDummyPlayer : BasePlayer
    {
        public override string Name { get; } = "AlwaysRaiseDummyPlayer_" + Guid.NewGuid();

        public override PlayerAction PostingBlind(IPostingBlindContext context)
        {
            throw new NotImplementedException();
        }

        public override PlayerAction GetTurn(IGetTurnContext context)
        {
            return PlayerAction.Raise(context.SmallBlind);
        }
    }
}

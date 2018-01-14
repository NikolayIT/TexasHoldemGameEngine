namespace TexasHoldem.AI.DummyPlayer
{
    using System;

    using TexasHoldem.Logic.Players;

    internal class AlwaysAllInDummyPlayer : BasePlayer
    {
        public override string Name { get; } = "AlwaysAllInDummyPlayer_" + Guid.NewGuid();

        public override PlayerAction PostingBlind(IPostingBlindContext context)
        {
            throw new NotImplementedException();
        }

        public override PlayerAction GetTurn(IGetTurnContext context)
        {
            if (context.MoneyLeft > 0)
            {
                return PlayerAction.Raise(context.MoneyLeft);
            }
            else
            {
                return PlayerAction.CheckOrCall();
            }
        }
    }
}

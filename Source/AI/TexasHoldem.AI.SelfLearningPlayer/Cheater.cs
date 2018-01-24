namespace TexasHoldem.AI.SelfLearningPlayer
{
    using System;

    using TexasHoldem.Logic.Players;

    public class Cheater : BasePlayer
    {
        public Cheater(IPlayingStyle trait)
        {
            throw new NotImplementedException();
        }

        public override string Name { get; } = "Cheater_" + Guid.NewGuid();

        public override int BuyIn { get; } = -1;

        public override PlayerAction PostingBlind(IPostingBlindContext context)
        {
            return context.BlindAction;
        }

        public override PlayerAction GetTurn(IGetTurnContext context)
        {
            throw new NotImplementedException();
        }
    }
}

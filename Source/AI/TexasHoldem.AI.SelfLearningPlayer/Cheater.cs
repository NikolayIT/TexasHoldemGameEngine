namespace TexasHoldem.AI.SelfLearningPlayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.AI.SelfLearningPlayer.PokerMath;
    using TexasHoldem.Logic.Players;

    public class Cheater : BasePlayer
    {
        private readonly IPlayingStyle trait;

        private ICalculator calculator;

        public Cheater(IPlayingStyle trait)
        {
            this.trait = trait;
        }

        public override string Name { get; } = "Cheater_" + Guid.NewGuid();

        public override int BuyIn { get; } = -1;

        public override PlayerAction PostingBlind(IPostingBlindContext context)
        {
            return context.BlindAction;
        }

        public override PlayerAction GetTurn(IGetTurnContext context)
        {
            var temp = this.calculator.Equity();
            return PlayerAction.CheckOrCall();
        }

        public override void StartRound(IStartRoundContext context)
        {
            base.StartRound(context);

            var list = new List<IPocket> { new Pocket(new[] { this.FirstCard, this.SecondCard }) };
            foreach (var item in context.HoleCardsOfOpponentsWhoAreInHand.Values)
            {
                list.Add(new Pocket(item.ToList()));
            }

            this.calculator = new Calculator(list, this.CommunityCards.ToList());
        }
    }
}

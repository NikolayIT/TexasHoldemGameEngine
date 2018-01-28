namespace TexasHoldem.AI.SelfLearningPlayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.AI.SelfLearningPlayer.PokerMath;
    using TexasHoldem.AI.SelfLearningPlayer.Strategy;
    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Players;

    public class Champion : BasePlayer
    {
        private readonly IPlayingStyle trait;

        public Champion(IPlayingStyle trait)
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
            var holeCardsOfOpponentsWhoAreInHand = new List<IPocket>();
            holeCardsOfOpponentsWhoAreInHand.Add(new Pocket(new[] { this.FirstCard, this.SecondCard }));
            var deadCards = new List<Card>();
            foreach (var item in context.Opponents)
            {
                if (item.InHand)
                {
                    holeCardsOfOpponentsWhoAreInHand.Add(new Pocket(item.HoleCards));
                }
                else
                {
                    deadCards.AddRange(item.HoleCards);
                }
            }

            var calculator = new Calculator(
                holeCardsOfOpponentsWhoAreInHand,
                deadCards,
                this.CommunityCards.ToList());
            var conduct = new Conduct(new[] { this.FirstCard, this.SecondCard }, context, calculator, this.trait);
            return conduct.OptimalAction();
        }
    }
}

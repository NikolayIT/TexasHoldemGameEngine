namespace TexasHoldem.Logic.Players
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;

    public abstract class BasePlayer : IPlayer
    {
        public abstract string Name { get; }

        protected IReadOnlyCollection<Card> CommunityCards { get; private set; }

        protected Card FirstCard { get; private set; }

        protected Card SecondCard { get; private set; }

        public virtual void StartGame(IStartGameContext context)
        {
        }

        public virtual void StartHand(IStartHandContext context)
        {
            this.FirstCard = context.FirstCard;
            this.SecondCard = context.SecondCard;
        }

        public virtual void StartRound(IStartRoundContext context)
        {
            this.CommunityCards = context.CommunityCards;
        }

        public abstract PlayerAction PostingBlind(IPostingBlindContext context);

        public abstract PlayerAction GetTurn(IGetTurnContext context);

        public virtual void EndRound(IEndRoundContext context)
        {
        }

        public virtual void EndHand(IEndHandContext context)
        {
        }

        public virtual void EndGame(IEndGameContext context)
        {
        }
    }
}

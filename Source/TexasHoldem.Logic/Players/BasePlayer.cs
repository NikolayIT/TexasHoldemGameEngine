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

        public virtual void StartGame(StartGameContext context)
        {
        }

        public virtual void StartHand(StartHandContext context)
        {
            this.FirstCard = context.FirstCard;
            this.SecondCard = context.SecondCard;
        }

        public virtual void StartRound(StartRoundContext context)
        {
            this.CommunityCards = context.CommunityCards;
        }

        public abstract PlayerAction GetTurn(GetTurnContext context);

        public virtual void EndRound(EndRoundContext context)
        {
        }

        public virtual void EndHand(EndHandContext context)
        {
        }

        public virtual void EndGame(EndGameContext context)
        {
        }
    }
}

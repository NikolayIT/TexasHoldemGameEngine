namespace TexasHoldem.Logic.Players
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;

    public abstract class BasePlayer : IPlayer
    {
        public abstract string Name { get; }

        protected int MoneyLeft { get; private set; }

        protected IReadOnlyCollection<Card> CommunityCards { get; private set; }

        public virtual void StartGame(StartGameContext context)
        {
            this.MoneyLeft = context.StartMoney;
        }

        public virtual void StartHand(StartHandContext context)
        {
        }

        public virtual void StartRound(StartRoundContext context)
        {
            this.CommunityCards = context.CommunityCards;
        }

        public abstract PlayerAction GetTurn(GetTurnContext context);

        public virtual void EndRound()
        {
        }

        public virtual void EndHand()
        {
        }

        public virtual void EndGame(EndGameContext context)
        {
        }
    }
}

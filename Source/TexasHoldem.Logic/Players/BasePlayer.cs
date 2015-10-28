namespace TexasHoldem.Logic.Players
{
    using System.Collections.Generic;

    public abstract class BasePlayer : IPlayer
    {
        public abstract string Name { get; }

        protected int MoneyLeft { get; set; }

        protected IReadOnlyCollection<string> PlayerNames { get; set; }

        public virtual void StartGame(StartGameContext context)
        {
            this.MoneyLeft = context.StartMoney;
            this.PlayerNames = context.OtherPlayerNames;
        }

        public virtual void StartHand(StartHandContext context)
        {
        }

        public virtual void StartRound(StartRoundContext context)
        {
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

namespace TexasHoldem.Logic.Players
{
    public abstract class BasePlayer : IPlayer
    {
        public abstract string Name { get; }

        protected int MoneyLeft { get; set; }

        public virtual void StartGame(StartGameContext context)
        {
            this.MoneyLeft = context.StartMoney;
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

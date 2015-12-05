namespace TexasHoldem.Logic.Players
{
    public abstract class PlayerDecorator : IPlayer
    {
        protected PlayerDecorator(IPlayer player)
        {
            this.Player = player;
        }

        public virtual string Name => this.Player.Name;

        protected IPlayer Player { get; }

        public virtual void StartGame(StartGameContext context)
        {
            this.Player.StartGame(context);
        }

        public virtual void StartHand(StartHandContext context)
        {
            this.Player.StartHand(context);
        }

        public virtual void StartRound(StartRoundContext context)
        {
            this.Player.StartRound(context);
        }

        public virtual PlayerAction GetTurn(GetTurnContext context)
        {
            return this.Player.GetTurn(context);
        }

        public virtual void EndRound(EndRoundContext context)
        {
            this.Player.EndRound(context);
        }

        public virtual void EndHand(EndHandContext context)
        {
            this.Player.EndHand(context);
        }

        public virtual void EndGame(EndGameContext context)
        {
            this.Player.EndGame(context);
        }
    }
}

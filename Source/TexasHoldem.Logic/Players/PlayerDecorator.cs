namespace TexasHoldem.Logic.Players
{
    public class PlayerDecorator : IPlayer
    {
        protected PlayerDecorator(IPlayer player)
        {
            this.Player = player;
        }

        public string Name => this.Player.Name;

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

        public virtual PlayerTurn GetTurn(GetTurnContext context)
        {
            return this.Player.GetTurn(context);
        }

        public virtual void EndRound()
        {
            this.Player.EndRound();
        }

        public virtual void EndHand()
        {
            this.Player.EndHand();
        }

        public virtual void EndGame(EndGameContext context)
        {
            this.Player.EndGame(context);
        }
    }
}

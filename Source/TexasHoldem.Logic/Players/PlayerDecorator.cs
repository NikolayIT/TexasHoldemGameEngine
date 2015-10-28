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

        public virtual void StartGame()
        {
            this.Player.StartGame();
        }

        public virtual void StartHand(StartHandContext context)
        {
            this.Player.StartHand(context);
        }

        public virtual void StartRound()
        {
            this.Player.StartRound();
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

        public virtual void EndGame()
        {
            this.Player.EndGame();
        }
    }
}

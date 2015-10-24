namespace TexasHoldem.Logic.Players
{
    using TexasHoldem.Logic.Cards;

    public class PlayerDecorator : IPlayer
    {
        protected PlayerDecorator(IPlayer player)
        {
            this.Player = player;
        }

        protected IPlayer Player { get; }

        public virtual void StartGame()
        {
            this.Player.StartGame();
        }

        public void StartHand(StartHandContext context)
        {
            this.Player.StartHand(context);
        }

        public PlayerTurn GetTurn(GetTurnContext context)
        {
            return this.Player.GetTurn(context);
        }

        public void EndHand()
        {
            this.Player.EndHand();
        }

        public void EndGame()
        {
            this.Player.EndGame();
        }
    }
}

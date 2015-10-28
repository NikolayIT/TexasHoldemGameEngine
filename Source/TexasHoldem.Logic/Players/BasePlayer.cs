namespace TexasHoldem.Logic.Players
{
    public abstract class BasePlayer : IPlayer
    {
        public abstract string Name { get; }

        public virtual void StartGame()
        {
        }

        public virtual void StartHand(StartHandContext context)
        {
        }

        public abstract PlayerTurn GetTurn(GetTurnContext context);

        public virtual void EndHand()
        {
        }

        public virtual void EndGame()
        {
        }
    }
}

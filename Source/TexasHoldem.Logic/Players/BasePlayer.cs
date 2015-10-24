namespace TexasHoldem.Logic.Players
{
    public abstract class BasePlayer : IPlayer
    {
        public abstract void StartGame();

        public abstract void StartHand(StartHandContext context);

        public abstract PlayerTurn GetTurn(GetTurnContext context);

        public abstract void EndHand();

        public abstract void EndGame();
    }
}

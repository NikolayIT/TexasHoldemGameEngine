namespace TexasHoldem.Logic.Players
{
    public interface IPlayer
    {
        void StartGame();

        void StartHand(StartHandContext context);

        PlayerTurn GetTurn(GetTurnContext context);

        void EndHand();

        void EndGame();
    }
}

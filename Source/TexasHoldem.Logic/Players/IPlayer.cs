namespace TexasHoldem.Logic.Players
{
    public interface IPlayer
    {
        string Name { get; }

        void StartGame();

        void StartHand(StartHandContext context);

        PlayerTurn GetTurn(GetTurnContext context);

        void EndHand();

        void EndGame();
    }
}

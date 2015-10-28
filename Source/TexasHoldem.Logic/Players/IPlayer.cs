namespace TexasHoldem.Logic.Players
{
    public interface IPlayer
    {
        string Name { get; }

        void StartGame();

        void StartHand(StartHandContext context);

        void StartRound();

        PlayerTurn GetTurn(GetTurnContext context);

        void EndRound();

        void EndHand();

        void EndGame();
    }
}

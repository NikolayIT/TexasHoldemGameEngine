namespace TexasHoldem.Logic.Players
{
    public interface IPlayer
    {
        string Name { get; }

        void StartGame(StartGameContext context);

        void StartHand(StartHandContext context);

        void StartRound(StartRoundContext context);

        PlayerTurn GetTurn(GetTurnContext context);

        void EndRound();

        void EndHand();

        void EndGame(EndGameContext context);
    }
}

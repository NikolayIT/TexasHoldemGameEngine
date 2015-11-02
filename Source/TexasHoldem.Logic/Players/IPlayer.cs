namespace TexasHoldem.Logic.Players
{
    public interface IPlayer
    {
        string Name { get; }

        void StartGame(StartGameContext context);

        void StartHand(StartHandContext context);

        void StartRound(StartRoundContext context);

        PlayerAction GetTurn(GetTurnContext context);

        void EndRound(EndRoundContext context);

        void EndHand(EndHandContext context);

        void EndGame(EndGameContext context);
    }
}

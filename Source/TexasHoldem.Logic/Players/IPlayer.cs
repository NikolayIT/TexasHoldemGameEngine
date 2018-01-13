namespace TexasHoldem.Logic.Players
{
    public interface IPlayer
    {
        string Name { get; }

        void StartGame(IStartGameContext context);

        void StartHand(IStartHandContext context);

        void StartRound(IStartRoundContext context);

        PlayerAction ToPostBlind(int stackSize, int blindSize, int currentPot);

        PlayerAction GetTurn(IGetTurnContext context);

        void EndRound(IEndRoundContext context);

        void EndHand(IEndHandContext context);

        void EndGame(IEndGameContext context);
    }
}

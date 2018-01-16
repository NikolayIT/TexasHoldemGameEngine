namespace TexasHoldem.Logic.Players
{
    public interface IPlayer
    {
        string Name { get; }

        int BuyIn { get; }

        void StartGame(IStartGameContext context);

        void StartHand(IStartHandContext context);

        void StartRound(IStartRoundContext context);

        PlayerAction PostingBlind(IPostingBlindContext context);

        PlayerAction GetTurn(IGetTurnContext context);

        void EndRound(IEndRoundContext context);

        void EndHand(IEndHandContext context);

        void EndGame(IEndGameContext context);
    }
}

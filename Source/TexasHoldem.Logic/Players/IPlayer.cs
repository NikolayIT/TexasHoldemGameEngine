namespace TexasHoldem.Logic.Players
{
    public interface IPlayer
    {
        void StartGame();

        void StartHand();

        PlayerTurn GetTurn(PlayerContext context);

        void EndHand();

        void EndGame();
    }
}

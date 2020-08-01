namespace TexasHoldem.Logic.GameMechanics
{
    using TexasHoldem.Logic.Players;

    public interface ITexasHoldemGame
    {
        int HandsPlayed { get; }

        IPlayer Start();
    }
}

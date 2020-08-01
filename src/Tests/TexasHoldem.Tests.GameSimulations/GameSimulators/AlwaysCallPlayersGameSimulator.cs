namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using TexasHoldem.AI.DummyPlayer;
    using TexasHoldem.Logic.Players;

    /// <summary>
    /// For performance profiling
    /// </summary>
    public class AlwaysCallPlayersGameSimulator : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new AlwaysCallDummyPlayer();
        private readonly IPlayer secondPlayer = new AlwaysCallDummyPlayer();

        protected override IPlayer GetFirstPlayer()
        {
            return this.firstPlayer;
        }

        protected override IPlayer GetSecondPlayer()
        {
            return this.secondPlayer;
        }
    }
}

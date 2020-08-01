namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using TexasHoldem.AI.SmartPlayer;
    using TexasHoldem.Logic.Players;

    public class SmartVsSmartPlayerSimulator : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new SmartPlayer();
        private readonly IPlayer secondPlayer = new SmartPlayer();

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

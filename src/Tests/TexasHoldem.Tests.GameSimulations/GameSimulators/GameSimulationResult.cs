namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using System;

    public class GameSimulationResult
    {
        public int FirstPlayerWins { get; set; }

        public int SecondPlayerWins { get; set; }

        public int HandsPlayed { get; set; }

        public TimeSpan SimulationDuration { get; set; }
    }
}

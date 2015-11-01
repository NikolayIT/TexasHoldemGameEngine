namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    public interface IGameSimulator
    {
        GameSimulationResult Simulate(int numberOfGames);
    }
}
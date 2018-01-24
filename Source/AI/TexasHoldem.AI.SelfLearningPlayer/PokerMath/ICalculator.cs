namespace TexasHoldem.AI.SelfLearningPlayer.PokerMath
{
    public interface ICalculator
    {
        double Equity();

        double EV(int pot, int wager);
    }
}

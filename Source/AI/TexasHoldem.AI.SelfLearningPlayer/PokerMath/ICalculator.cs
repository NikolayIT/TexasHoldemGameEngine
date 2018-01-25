namespace TexasHoldem.AI.SelfLearningPlayer.PokerMath
{
    public interface ICalculator
    {
        double Equity();

        double BetToNeutralEV(int pot);
    }
}

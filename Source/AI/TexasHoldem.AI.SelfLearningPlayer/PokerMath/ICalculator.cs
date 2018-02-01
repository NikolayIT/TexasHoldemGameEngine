namespace TexasHoldem.AI.SelfLearningPlayer.PokerMath
{
    using System.Collections.Generic;

    public interface ICalculator
    {
        ICollection<HandStrength> Equity();
    }
}

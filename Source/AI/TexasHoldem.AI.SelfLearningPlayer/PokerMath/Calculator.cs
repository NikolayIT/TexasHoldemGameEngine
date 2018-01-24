namespace TexasHoldem.AI.SelfLearningPlayer.PokerMath
{
    using System;
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;

    public class Calculator : ICalculator
    {
        public Calculator(
            ICollection<Card> heroHoleCards, ISet<ICollection<Card>> opponentsHoleCards, ICollection<Card> communityCards)
        {
            throw new NotImplementedException();
        }

        public double Equity()
        {
            throw new NotImplementedException();
        }

        public double EV(int pot, int wager)
        {
            throw new NotImplementedException();
        }
    }
}

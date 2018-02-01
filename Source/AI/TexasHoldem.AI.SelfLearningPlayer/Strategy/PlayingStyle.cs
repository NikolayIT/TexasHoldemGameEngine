namespace TexasHoldem.AI.SelfLearningPlayer.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.AI.SelfLearningPlayer.PokerMath;

    public class PlayingStyle : IPlayingStyle
    {
        public PlayingStyle(double vpip, double pfr, double preflopThreeBet)
        {
            this.VPIP = vpip;
            this.PFR = pfr;
            this.PreflopThreeBet = preflopThreeBet;
        }

        public double VPIP { get; }

        public double PFR { get; }

        public double PreflopThreeBet { get; }
    }
}

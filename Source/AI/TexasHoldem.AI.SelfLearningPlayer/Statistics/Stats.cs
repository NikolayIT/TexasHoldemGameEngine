namespace TexasHoldem.AI.SelfLearningPlayer.Statistics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.AI.SelfLearningPlayer.Strategy;
    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Players;

    public class Stats : IPlayingStyle
    {
        public double VPIP
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public double PFR
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public double PreflopThreeBet
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Callers { get; private set; }

        public bool OpenRaiseOpportunity { get; private set; }

        public bool PreflopThreeBetOpportunity { get; private set; }

        public void Update(GameRoundType round, IReadOnlyCollection<PlayerActionAndName> previousRoundActions)
        {
            if (round == GameRoundType.PreFlop)
            {
                var reverse = previousRoundActions.Reverse();
                this.Callers = reverse.Count(
                    x => x.Action.Type == PlayerActionType.CheckCall && x.Action.Type != PlayerActionType.Raise);

                var raises = previousRoundActions.Count(x => x.Action.Type == PlayerActionType.Raise);
                this.OpenRaiseOpportunity = raises == 0;
                this.PreflopThreeBetOpportunity = raises == 1;
            }
        }
    }
}

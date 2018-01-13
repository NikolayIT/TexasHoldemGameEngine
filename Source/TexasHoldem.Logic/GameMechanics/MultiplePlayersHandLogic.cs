namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Helpers;
    using TexasHoldem.Logic.Players;

    internal class MultiplePlayersHandLogic : BaseHandLogic
    {
        public MultiplePlayersHandLogic(IList<IInternalPlayer> players, int handNumber, int smallBlind)
            : base(players, handNumber, smallBlind, new MultiplePlayersBettingLogic(players, smallBlind))
        {
        }

        protected override void DetermineWinnerAndAddPot(int pot)
        {
            throw new System.NotImplementedException();
        }
    }
}
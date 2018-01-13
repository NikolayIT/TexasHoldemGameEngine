namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Helpers;
    using TexasHoldem.Logic.Players;

    internal class HeadsUpHandLogic : BaseHandLogic
    {
        public HeadsUpHandLogic(IList<IInternalPlayer> players, int handNumber, int smallBlind)
            : base(players, handNumber, smallBlind, new HeadsUpBettingLogic(players, smallBlind))
        {
        }

        protected override void DetermineWinnerAndAddPot(int pot)
        {
            if (this.Players.Count(x => x.PlayerMoney.InHand) == 1)
            {
                var winner = this.Players.FirstOrDefault(x => x.PlayerMoney.InHand);
                winner.PlayerMoney.Money += pot;
            }
            else
            {
                // showdown
                foreach (var player in this.Players)
                {
                    if (player.PlayerMoney.InHand)
                    {
                        this.ShowdownCards.Add(player.Name, player.Cards);
                    }
                }

                var betterHand = Helpers.CompareCards(
                    this.Players[0].Cards.Concat(this.CommunityCards),
                    this.Players[1].Cards.Concat(this.CommunityCards));
                if (betterHand > 0)
                {
                    this.Players[0].PlayerMoney.Money += pot;
                }
                else if (betterHand < 0)
                {
                    this.Players[1].PlayerMoney.Money += pot;
                }
                else
                {
                    this.Players[0].PlayerMoney.Money += pot / 2;
                    this.Players[1].PlayerMoney.Money += pot / 2;
                }
            }
        }
    }
}

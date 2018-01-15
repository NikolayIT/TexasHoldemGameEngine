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

                // Bubble sort
                var players = this.Players.ToArray();
                for (int element = 0; element < players.Length - 1; element++)
                {
                    for (int i = 0; i < players.Length - (element + 1); i++)
                    {
                        if (Helpers.CompareCards(
                            players[i].Cards.Concat(this.CommunityCards),
                            players[i + 1].Cards.Concat(this.CommunityCards)) < 0)
                        {
                            var temp = players[i + 1];
                            players[i + 1] = players[i];
                            players[i] = temp;
                        }
                    }
                }

                int next = 1;
                do
                {
                    var betterHand = Helpers.CompareCards(
                        players[0].Cards.Concat(this.CommunityCards),
                        players[next].Cards.Concat(this.CommunityCards));
                    if (betterHand > 0)
                    {
                        if (next == 1)
                        {
                            players[0].PlayerMoney.Money += pot;
                            break;
                        }
                    }
                    else if (betterHand == 0)
                    {
                        next++;
                        if (next <= players.Count() - 1)
                        {
                            continue;
                        }
                    }

                    for (int i = 0; i < next; i++)
                    {
                        // Divide the pot among two or more players
                        players[i].PlayerMoney.Money += pot / next;
                    }
                }
                while (true);
            }
        }
    }
}
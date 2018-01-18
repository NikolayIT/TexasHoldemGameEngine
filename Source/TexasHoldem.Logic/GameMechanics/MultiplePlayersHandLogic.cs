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

        protected override void DetermineWinnerAndAddPot(int pot, IReadOnlyCollection<SidePot> sidePot)
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
                var playersInHand = this.Players.Where(p => p.PlayerMoney.InHand).ToArray();
                for (int element = 0; element < playersInHand.Length - 1; element++)
                {
                    for (int i = 0; i < playersInHand.Length - (element + 1); i++)
                    {
                        if (Helpers.CompareCards(
                            playersInHand[i].Cards.Concat(this.CommunityCards),
                            playersInHand[i + 1].Cards.Concat(this.CommunityCards)) > 0)
                        {
                            var temp = playersInHand[i + 1];
                            playersInHand[i + 1] = playersInHand[i];
                            playersInHand[i] = temp;
                        }
                    }
                }

                Stack<List<IInternalPlayer>> sortedByHandStrength = new Stack<List<IInternalPlayer>>();
                sortedByHandStrength.Push(new List<IInternalPlayer> { playersInHand[0] });
                for (int i = 0; i < playersInHand.Count() - 1; i++)
                {
                    var betterHand = Helpers.CompareCards(
                        playersInHand[i].Cards.Concat(this.CommunityCards),
                        playersInHand[i + 1].Cards.Concat(this.CommunityCards));

                    if (betterHand < 0)
                    {
                        sortedByHandStrength.Push(new List<IInternalPlayer> { playersInHand[i + 1] });
                    }
                    else if (betterHand == 0)
                    {
                        sortedByHandStrength.Peek().Add(playersInHand[i + 1]);
                    }
                }

                var remainingPots = sidePot.ToList();
                remainingPots.Add(
                    new SidePot(
                        pot - remainingPots.Sum(x => x.Amount),
                        playersInHand.Where(x => x.PlayerMoney.Money > 0)
                            .Select(x => x.Name)
                            .ToList()
                            .AsReadOnly()));

                do
                {
                    var winners = sortedByHandStrength.Pop();

                    var unusedPots = new List<SidePot>();

                    foreach (var oneOfThePots in remainingPots)
                    {
                        var applicantsForThePot = oneOfThePots.NamesOfParticipants.Intersect(winners.Select(s => s.Name));
                        var count = applicantsForThePot.Count();
                        if (count > 0)
                        {
                            var prize = oneOfThePots.Amount / count;
                            foreach (var applicant in applicantsForThePot)
                            {
                                winners.First(x => x.Name == applicant).PlayerMoney.Money += prize;
                            }
                        }
                        else
                        {
                            unusedPots.Add(oneOfThePots);
                        }
                    }

                    remainingPots = unusedPots;
                }
                while (remainingPots.Count > 0);
            }
        }
    }
}
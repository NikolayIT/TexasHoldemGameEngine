namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Players;

    internal class MultiplePlayersBettingLogic : BaseBettingLogic
    {
        private List<int> boundsOfSidePots;

        private int lowerBound;

        public MultiplePlayersBettingLogic(IList<IInternalPlayer> players, int smallBlind)
            : base(players, smallBlind)
        {
            this.boundsOfSidePots = new List<int>();
            this.lowerBound = 0;
        }

        public override void Bet(GameRoundType gameRoundType)
        {
            this.RoundBets.Clear();
            var playerIndex = 1;

            if (gameRoundType == GameRoundType.PreFlop)
            {
                this.PlaceBlinds();
                playerIndex = 3;
            }

            while (this.AllPlayers.Count(x => x.PlayerMoney.InHand) >= 2
                   && this.AllPlayers.Any(x => x.PlayerMoney.ShouldPlayInRound))
            {
                var player = this.AllPlayers[playerIndex % this.AllPlayers.Count];
                if (player.PlayerMoney.Money <= 0)
                {
                    // Players who are all-in are not involved in betting round
                    player.PlayerMoney.ShouldPlayInRound = false;
                    playerIndex++;
                    continue;
                }

                if (!player.PlayerMoney.InHand || !player.PlayerMoney.ShouldPlayInRound)
                {
                    if (player.PlayerMoney.InHand == player.PlayerMoney.ShouldPlayInRound)
                    {
                        playerIndex++;
                    }

                    continue;
                }

                var maxMoneyPerPlayer = this.AllPlayers.Max(x => x.PlayerMoney.CurrentRoundBet);
                var action =
                    player.GetTurn(
                        new GetTurnContext(
                            gameRoundType,
                            this.RoundBets.AsReadOnly(),
                            this.SmallBlind,
                            player.PlayerMoney.Money,
                            this.Pot,
                            player.PlayerMoney.CurrentRoundBet,
                            maxMoneyPerPlayer,
                            this.MinRaise(maxMoneyPerPlayer, player.Name)));

                action = player.PlayerMoney.DoPlayerAction(action, maxMoneyPerPlayer);
                this.RoundBets.Add(new PlayerActionAndName(player.Name, action));

                if (action.Type == PlayerActionType.Raise)
                {
                    // When raising, all players are required to do action afterwards in current round
                    foreach (var playerToUpdate in this.AllPlayers)
                    {
                        playerToUpdate.PlayerMoney.ShouldPlayInRound = playerToUpdate.PlayerMoney.InHand ? true : false;
                    }
                }

                if (player.PlayerMoney.Money <= 0)
                {
                    this.boundsOfSidePots.Add(player.PlayerMoney.CurrentlyInPot);
                }

                player.PlayerMoney.ShouldPlayInRound = false;
                playerIndex++;
            }

            foreach (var upperBound in this.boundsOfSidePots.Where(x => x > this.lowerBound).OrderBy(x => x))
            {
                var namesOfParticipants = new List<string>();
                var amount = 0;
                foreach (var item in this.AllPlayers)
                {
                    if (item.PlayerMoney.CurrentlyInPot > this.lowerBound)
                    {
                        // The player participates in the side pot
                        namesOfParticipants.Add(item.Name);
                        if (item.PlayerMoney.CurrentlyInPot >= upperBound)
                        {
                            amount += upperBound - this.lowerBound;
                        }
                        else
                        {
                            amount += item.PlayerMoney.CurrentlyInPot - this.lowerBound;
                        }
                    }
                }

                this.CreateSidePot(new SidePot(
                    amount,
                    namesOfParticipants));
                this.lowerBound = upperBound;
            }

            // A method is needed that will return uncalled bet!!!
            // this.ReturnMoneyInCaseOfAllIn();
        }

        protected override void PlaceBlinds()
        {
            // Small blind
            this.RoundBets.Add(
                new PlayerActionAndName(
                    this.AllPlayers[1].Name,
                    this.AllPlayers[1].PostingBlind(
                        new PostingBlindContext(
                            this.AllPlayers[1].PlayerMoney.DoPlayerAction(PlayerAction.Post(this.SmallBlind), 0),
                            0,
                            this.AllPlayers[1].PlayerMoney.Money))));

            // Big blind
            this.RoundBets.Add(
                new PlayerActionAndName(
                    this.AllPlayers[2].Name,
                    this.AllPlayers[2].PostingBlind(
                        new PostingBlindContext(
                            this.AllPlayers[2].PlayerMoney.DoPlayerAction(PlayerAction.Post(2 * this.SmallBlind), 0),
                            this.Pot,
                            this.AllPlayers[2].PlayerMoney.Money))));
        }
    }
}
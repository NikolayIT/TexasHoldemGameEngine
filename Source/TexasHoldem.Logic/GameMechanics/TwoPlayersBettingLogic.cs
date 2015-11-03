namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Players;

    internal class TwoPlayersBettingLogic
    {
        private readonly IList<InternalPlayer> allPlayers;

        private readonly int smallBlind;

        public TwoPlayersBettingLogic(IList<InternalPlayer> players, int smallBlind)
        {
            this.allPlayers = players;
            this.smallBlind = smallBlind;
            this.RoundBets = new List<PlayerActionAndName>();
        }

        public int Pot
        {
            get
            {
                return this.allPlayers.Sum(x => x.PlayerMoney.CurrentlyInPot);
            }
        }

        public List<PlayerActionAndName> RoundBets { get; }

        public void Bet(GameRoundType gameRoundType)
        {
            this.RoundBets.Clear();
            var playerIndex = 0;

            if (gameRoundType == GameRoundType.PreFlop)
            {
                this.PlaceBlinds();
                playerIndex = 2;
            }

            while (this.allPlayers.Count(x => x.PlayerMoney.InHand) >= 2
                   && this.allPlayers.Any(x => x.PlayerMoney.ShouldPlayInRound))
            {
                var player = this.allPlayers[playerIndex % this.allPlayers.Count];
                if (!player.PlayerMoney.InHand || !player.PlayerMoney.ShouldPlayInRound)
                {
                    continue;
                }

                var maxMoneyPerPlayer = this.allPlayers.Max(x => x.PlayerMoney.CurrentRoundBet);
                var action =
                    player.GetTurn(
                        new GetTurnContext(
                            gameRoundType,
                            this.RoundBets.AsReadOnly(),
                            this.smallBlind,
                            player.PlayerMoney.Money,
                            this.Pot,
                            player.PlayerMoney.CurrentRoundBet,
                            maxMoneyPerPlayer));

                action = player.PlayerMoney.DoPlayerAction(action, maxMoneyPerPlayer);
                this.RoundBets.Add(new PlayerActionAndName(player.Name, action));

                if (action.Type == PlayerActionType.Raise)
                {
                    // When raising, all players are required to do action afterwards in current round
                    foreach (var playerToUpdate in this.allPlayers)
                    {
                        playerToUpdate.PlayerMoney.ShouldPlayInRound = true;
                    }
                }

                player.PlayerMoney.ShouldPlayInRound = false;
                playerIndex++;
            }

            this.ReturnMoneyInCaseOfAllIn();
        }

        private void PlaceBlinds()
        {
            var smallBlindAction = PlayerAction.Raise(this.smallBlind);

            // Small blind
            this.RoundBets.Add(
                new PlayerActionAndName(
                    this.allPlayers[0].Name,
                    this.allPlayers[0].PlayerMoney.DoPlayerAction(smallBlindAction, 0)));

            // Big blind
            this.allPlayers[1].PlayerMoney.DoPlayerAction(smallBlindAction, this.smallBlind);
            this.RoundBets.Add(
                new PlayerActionAndName(
                    this.allPlayers[1].Name,
                    this.allPlayers[0].PlayerMoney.DoPlayerAction(smallBlindAction, 0)));
        }

        private void ReturnMoneyInCaseOfAllIn()
        {
            var minMoneyPerPlayer = this.allPlayers.Min(x => x.PlayerMoney.CurrentRoundBet);
            foreach (var player in this.allPlayers)
            {
                player.PlayerMoney.NormalizeBets(minMoneyPerPlayer);
            }
        }
    }
}

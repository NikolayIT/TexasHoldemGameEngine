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
        }

        public int Pot
        {
            get
            {
                return this.allPlayers.Sum(x => x.PlayerMoney.CurrentlyInPot);
            }
        }

        public void Bet(GameRoundType gameRoundType)
        {
            var bets = new List<PlayerActionAndName>();
            if (gameRoundType == GameRoundType.PreFlop)
            {
                var smallBlindAction = PlayerAction.Raise(this.smallBlind);

                // Small blind
                this.allPlayers[0].PlayerMoney.DoPlayerAction(smallBlindAction, 0);
                bets.Add(new PlayerActionAndName(this.allPlayers[0].Name, PlayerAction.Raise(this.smallBlind)));

                // Big blind
                this.allPlayers[1].PlayerMoney.DoPlayerAction(smallBlindAction, this.smallBlind);
                bets.Add(new PlayerActionAndName(this.allPlayers[1].Name, PlayerAction.Raise(this.smallBlind)));
            }

            var playerIndex = 1;
            while (this.allPlayers.Count(x => x.PlayerMoney.InHand) >= 2 && this.allPlayers.Any(x => x.PlayerMoney.ShouldPlayInRound))
            {
                playerIndex++;
                var player = this.allPlayers[playerIndex % this.allPlayers.Count];
                if (!player.PlayerMoney.InHand)
                {
                    continue;
                }

                var maxMoneyPerPlayer = this.allPlayers.Max(x => x.PlayerMoney.CurrentRoundBet);
                var action =
                    player.GetTurn(
                        new GetTurnContext(
                            gameRoundType,
                            bets.AsReadOnly(),
                            this.smallBlind,
                            player.PlayerMoney.Money,
                            this.Pot,
                            player.PlayerMoney.CurrentlyInPot,
                            maxMoneyPerPlayer));

                action = player.PlayerMoney.DoPlayerAction(action, maxMoneyPerPlayer);
                if (action.Type == PlayerActionType.Raise)
                {
                    foreach (var playerToUpdate in this.allPlayers)
                    {
                        playerToUpdate.PlayerMoney.ShouldPlayInRound = true;
                    }
                }

                bets.Add(new PlayerActionAndName(player.Name, action));
                player.PlayerMoney.ShouldPlayInRound = false;
            }
        }
    }
}

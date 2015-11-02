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

        public List<PlayerActionAndName> Bets { get; private set; }

        public void Bet(GameRoundType gameRoundType)
        {
            this.Bets = new List<PlayerActionAndName>();

            var playerIndex = 0;
            if (gameRoundType == GameRoundType.PreFlop)
            {
                var smallBlindAction = PlayerAction.Raise(this.smallBlind);

                // Small blind
                this.Bets.Add(
                    new PlayerActionAndName(
                        this.allPlayers[0].Name,
                        this.allPlayers[0].PlayerMoney.DoPlayerAction(smallBlindAction, 0)));
                playerIndex++;

                // Big blind
                this.allPlayers[1].PlayerMoney.DoPlayerAction(smallBlindAction, this.smallBlind);
                this.Bets.Add(
                    new PlayerActionAndName(
                        this.allPlayers[1].Name,
                        this.allPlayers[0].PlayerMoney.DoPlayerAction(smallBlindAction, 0)));
                playerIndex++;
            }

            while (this.allPlayers.Count(x => x.PlayerMoney.InHand) >= 2
                   && this.allPlayers.Any(x => x.PlayerMoney.ShouldPlayInRound))
            {
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
                            this.Bets.AsReadOnly(),
                            this.smallBlind,
                            player.PlayerMoney.Money,
                            this.Pot,
                            player.PlayerMoney.CurrentlyInPot,
                            maxMoneyPerPlayer));

                action = player.PlayerMoney.DoPlayerAction(action, maxMoneyPerPlayer);
                this.Bets.Add(new PlayerActionAndName(player.Name, action));

                if (action.Type == PlayerActionType.Raise)
                {
                    // When raising, all players are required to do action
                    foreach (var playerToUpdate in this.allPlayers)
                    {
                        playerToUpdate.PlayerMoney.ShouldPlayInRound = true;
                    }
                }

                player.PlayerMoney.ShouldPlayInRound = false;
                playerIndex++;
            }
        }
    }
}

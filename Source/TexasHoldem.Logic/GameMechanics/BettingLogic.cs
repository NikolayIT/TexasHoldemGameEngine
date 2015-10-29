namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Helpers;
    using TexasHoldem.Logic.Players;

    internal class BettingLogic
    {
        private readonly IList<InternalPlayer> allPlayers;

        private readonly int smallBlind;

        public BettingLogic(IList<InternalPlayer> players, int smallBlind)
        {
            this.allPlayers = players;
            this.smallBlind = smallBlind;
            this.Pot = 0;
        }

        public int Pot { get; private set; }

        public void Bet(GameRoundType gameRoundType)
        {
            var bets = new List<PlayerActionAndName>();
            if (gameRoundType == GameRoundType.PreFlop)
            {
                // TODO: What if small blind is bigger than player's money?
                this.Bet(this.allPlayers[0], this.smallBlind);
                bets.Add(new PlayerActionAndName(this.allPlayers[0].Name, PlayerAction.Raise(this.smallBlind)));

                // TODO: What if big blind is bigger than player's money?
                this.Bet(this.allPlayers[1], this.smallBlind * 2);
                bets.Add(new PlayerActionAndName(this.allPlayers[1].Name, PlayerAction.Raise(this.smallBlind * 2)));
            }

            var playerIndex = 1;
            while (this.allPlayers.Count(x => x.InHand) >= 2 && this.allPlayers.Any(x => x.ShouldPlayInRound))
            {
                playerIndex++;
                var player = this.allPlayers[playerIndex % this.allPlayers.Count];
                if (!player.InHand)
                {
                    continue;
                }

                var maxMoneyPerPlayer = this.allPlayers.Max(x => x.CurrentlyInPot);
                var action =
                    player.GetTurn(
                        new GetTurnContext(
                            gameRoundType,
                            bets.AsReadOnly(),
                            this.smallBlind,
                            this.Pot,
                            maxMoneyPerPlayer));

                bets.Add(new PlayerActionAndName(player.Name, action));

                if (action.Type == PlayerActionType.Raise)
                {
                    foreach (var playerToUpdate in this.allPlayers)
                    {
                        playerToUpdate.ShouldPlayInRound = true;
                    }

                    this.Bet(player, action.Money);
                }
                else if (action.Type == PlayerActionType.CheckCall)
                {
                    player.ShouldPlayInRound = true;
                    var amountToCall = this.allPlayers.Max(x => x.CurrentlyInPot) - player.CurrentlyInPot;
                    this.Bet(player, amountToCall);
                }
                else //// PlayerActionType.Fold
                {
                    player.InHand = false;
                }

                player.ShouldPlayInRound = false;
            }
        }

        private void Bet(InternalPlayer player, int amount)
        {
            player.Bet(amount);
            this.Pot += amount;
        }
    }
}

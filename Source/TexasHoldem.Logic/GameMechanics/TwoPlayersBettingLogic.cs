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
                return this.allPlayers.Sum(x => x.CurrentlyInPot);
            }
        }

        public void Bet(GameRoundType gameRoundType)
        {
            var bets = new List<PlayerActionAndName>();
            if (gameRoundType == GameRoundType.PreFlop)
            {
                // TODO: What if small blind is bigger than player's money?
                this.allPlayers[0].PlaceMoney(this.smallBlind);
                bets.Add(new PlayerActionAndName(this.allPlayers[0].Name, PlayerAction.Raise(this.smallBlind)));

                // TODO: What if big blind is bigger than player's money?
                this.allPlayers[1].PlaceMoney(this.smallBlind * 2);
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

                var maxMoneyPerPlayer = this.allPlayers.Max(x => x.CurrentRoundBet);
                var action =
                    player.GetTurn(
                        new GetTurnContext(
                            gameRoundType,
                            bets.AsReadOnly(),
                            this.smallBlind,
                            player.Money,
                            this.Pot,
                            player.CurrentlyInPot,
                            maxMoneyPerPlayer));

                action = this.DoPlayerAction(player, action, maxMoneyPerPlayer);

                bets.Add(new PlayerActionAndName(player.Name, action));
                player.ShouldPlayInRound = false;
            }
        }

        private PlayerAction DoPlayerAction(InternalPlayer player, PlayerAction action, int maxMoneyPerPlayer)
        {
            if (action.Type == PlayerActionType.Raise)
            {
                player.Call(maxMoneyPerPlayer);

                if (player.Money <= 0)
                {
                    return PlayerAction.CheckOrCall();
                }

                foreach (var playerToUpdate in this.allPlayers)
                {
                    playerToUpdate.ShouldPlayInRound = true;
                }

                // TODO: Min raise?
                if (player.Money > action.Money)
                {
                    player.PlaceMoney(action.Money);
                }
                else
                {
                    // All-in
                    action.Money = player.Money;
                    player.PlaceMoney(action.Money);
                }
            }
            else if (action.Type == PlayerActionType.CheckCall)
            {
                player.Call(maxMoneyPerPlayer);
            }
            else //// PlayerActionType.Fold
            {
                player.InHand = false;
            }

            return action;
        }
    }
}

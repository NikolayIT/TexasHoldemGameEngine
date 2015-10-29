namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Helpers;
    using TexasHoldem.Logic.Players;

    internal class BettingLogic
    {
        private static IActionValidator actionValidator = new ActionValidator();

        private readonly IList<InternalPlayer> allPlayers;

        private readonly int smallBlind;

        private int pot;

        public BettingLogic(IList<InternalPlayer> players, int smallBlind)
        {
            this.allPlayers = players;
            this.smallBlind = smallBlind;
            this.pot = 0;
        }

        public void Start(GameRoundType gameRoundType, ICollection<Card> communityCards)
        {
            var bets = new List<PlayerActionAndName>();
            if (gameRoundType == GameRoundType.PreFlop)
            {
                this.Bet(this.allPlayers[0], this.smallBlind);
                bets.Add(new PlayerActionAndName(this.allPlayers[0].Name, PlayerAction.Raise(this.smallBlind)));

                this.Bet(this.allPlayers[1], this.smallBlind * 2);
                bets.Add(new PlayerActionAndName(this.allPlayers[1].Name, PlayerAction.Raise(this.smallBlind * 2)));
            }

            var potBeforeRound = this.pot;
            var playerIndex = 1;
            while (this.allPlayers.Count(x => x.InHand) >= 2 && this.allPlayers.Any(x => x.ShouldPlayInRound))
            {
                playerIndex++;
                var player = this.allPlayers[playerIndex % this.allPlayers.Count];
                if (!player.InHand)
                {
                    continue;
                }

                var action =
                    player.GetTurn(
                        new GetTurnContext(communityCards, gameRoundType, potBeforeRound, bets.AsReadOnly(), this.pot));

                bets.Add(new PlayerActionAndName(player.Name, action));

                if (action.Type == PlayerActionType.Raise)
                {
                    foreach (var playerToUpdate in this.allPlayers)
                    {
                        playerToUpdate.ShouldPlayInRound = true;
                    }

                    this.Bet(player, action.Money);
                }
                else if (action.Type == PlayerActionType.Call)
                {
                    player.ShouldPlayInRound = true;
                    this.Bet(player, action.Money);
                }
                else if (action.Type == PlayerActionType.Check)
                {
                    // TODO: Is OK to check?
                    player.ShouldPlayInRound = true;
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
            // TODO: What if small blind is bigger than player's money?
            player.Bet(amount);
            this.pot += amount;
        }
    }
}

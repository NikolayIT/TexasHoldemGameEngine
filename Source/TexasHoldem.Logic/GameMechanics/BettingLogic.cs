namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;

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
            var potBeforeRound = this.pot;
            var playerIndex = 0;

            if (gameRoundType == GameRoundType.PreFlop)
            {
                this.Bet(this.allPlayers[0], this.smallBlind);
                bets.Add(new PlayerActionAndName(this.allPlayers[0].Name, PlayerAction.Raise(this.smallBlind)));
                playerIndex++;

                this.Bet(this.allPlayers[1], this.smallBlind * 2);
                bets.Add(new PlayerActionAndName(this.allPlayers[1].Name, PlayerAction.Raise(this.smallBlind * 2)));
                playerIndex++;
            }

            while (true)
            {
                var player = this.allPlayers[playerIndex % this.allPlayers.Count];
                var getTurnContext = new GetTurnContext(
                    communityCards,
                    gameRoundType,
                    potBeforeRound,
                    bets.AsReadOnly(),
                    this.pot);
                var action = player.GetTurn(getTurnContext);

                bets.Add(new PlayerActionAndName(player.Name, action));

                if (action.Type == PlayerActionType.Raise)
                {
                    this.Bet(player, action.Money);
                }
                else if (action.Type == PlayerActionType.Call)
                {
                    this.Bet(player, action.Money);
                }
                else if (action.Type == PlayerActionType.Check)
                {
                    // TODO: Is OK to check?
                }
                else
                {
                    // Fold
                    break;
                }

                playerIndex++;
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

namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Players;

    internal class HeadsUpBettingLogic : BaseBettingLogic
    {
        public HeadsUpBettingLogic(IList<IInternalPlayer> players, int smallBlind)
            : base(players, smallBlind)
        {
        }

        public override void Bet(GameRoundType gameRoundType)
        {
            this.RoundBets.Clear();
            var playerIndex = 0;

            if (gameRoundType == GameRoundType.PreFlop)
            {
                this.PlaceBlinds();
                playerIndex = 2;
            }

            while (this.AllPlayers.Count(x => x.PlayerMoney.InHand) >= 2
                   && this.AllPlayers.Any(x => x.PlayerMoney.ShouldPlayInRound))
            {
                var player = this.AllPlayers[playerIndex % this.AllPlayers.Count];
                if (!player.PlayerMoney.InHand || !player.PlayerMoney.ShouldPlayInRound)
                {
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
                        playerToUpdate.PlayerMoney.ShouldPlayInRound = true;
                    }
                }

                player.PlayerMoney.ShouldPlayInRound = false;
                playerIndex++;
            }

            this.ReturnMoneyInCaseOfAllIn();
        }
    }
}
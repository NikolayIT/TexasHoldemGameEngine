namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Players;

    internal abstract class BaseBettingLogic
    {
        private readonly IList<IInternalPlayer> allPlayers;

        private readonly int smallBlind;

        public BaseBettingLogic(IList<IInternalPlayer> players, int smallBlind)
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

        protected IReadOnlyList<IInternalPlayer> AllPlayers
        {
            get
            {
                return this.allPlayers.ToList();
            }
        }

        protected int SmallBlind
        {
            get
            {
                return this.smallBlind;
            }
        }

        public abstract void Bet(GameRoundType gameRoundType);

        protected void PlaceBlinds()
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

        protected void ReturnMoneyInCaseOfAllIn()
        {
            var minMoneyPerPlayer = this.allPlayers.Min(x => x.PlayerMoney.CurrentRoundBet);
            foreach (var player in this.allPlayers)
            {
                player.PlayerMoney.NormalizeBets(minMoneyPerPlayer);
            }
        }
    }
}
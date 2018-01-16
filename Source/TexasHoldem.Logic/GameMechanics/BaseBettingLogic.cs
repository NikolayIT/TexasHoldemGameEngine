namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Players;

    internal abstract class BaseBettingLogic
    {
        private readonly IList<IInternalPlayer> allPlayers;

        private readonly int smallBlind;

        private int lastRoundBet;

        private int lastStepBet;

        private string aggressorName;

        public BaseBettingLogic(IList<IInternalPlayer> players, int smallBlind)
        {
            this.allPlayers = players;
            this.smallBlind = smallBlind;
            this.RoundBets = new List<PlayerActionAndName>();

            this.lastRoundBet = 2 * smallBlind; // Big blind
            this.lastStepBet = this.lastRoundBet;
            this.aggressorName = string.Empty;
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

        protected virtual void PlaceBlinds()
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

        protected int MinRaise(int maxMoneyPerPlayer, string currentPlayerName)
        {
            /*
             * MinRaise =
             *      The investment of the current aggressor
             *      +
             *      (The investment of the current aggressor - The investment of the previous aggressor)
             * Examples:
             * 1. SB->5$; BB->10$; UTG->raise 35$; MP->MinRaise=60$ (35$ + 25$)
             * 2. SB->5$; BB->10$; UTG->raise 35$; MP->re-raise 150$; CO->call 150$; BTN->MinRaise=265$ (150$ + 115$)
             * 3. SB->5$; BB->10$; UTG->raise 12$(ALL-IN); MP->MinRaise=22$ (12$ + 10$(BB is step of initial raiser))
            */

            if (maxMoneyPerPlayer == 0)
            {
                // Start postflop. Players did not bet.
                this.lastRoundBet = 0;
                this.lastStepBet = 2 * this.smallBlind; // Big blind
                this.aggressorName = string.Empty;
            }

            if (this.aggressorName == currentPlayerName)
            {
                /*
                 * SB->5$; BB->10$; BTN->raise 32$; SB->re-raise 34$(ALL-IN); BB->call 34$; BTN->MinRaise=-1$ (SB has made a not full raise);
                 * Since no players completed a full raise over BTN's initial raise,
                 * neither BTN nor BB are allowed to re-raise here. Their only options are to call the 34$, or fold.
                */
                return -1;
            }

            if (maxMoneyPerPlayer > this.lastRoundBet)
            {
                // We check for the fullness of the raise
                if (this.lastRoundBet + this.lastStepBet <= maxMoneyPerPlayer)
                {
                    this.lastStepBet = maxMoneyPerPlayer - this.lastRoundBet;
                    this.lastRoundBet = maxMoneyPerPlayer;
                    this.aggressorName = this.RoundBets.Last().PlayerName;
                }
                else
                {
                    /*
                     * For example, we are sitting on CO
                     * SB->5$; BB->10$; UTG->raise 35$; MP->re-raise 37$(ALL-IN); CO->MinRaise 62$ (37$ + 25$(UTG is step of initial raiser))
                     * then maxMoneyPerPlayer=37$ and MinRaise=maxMoneyPerPlayer+step(25$)
                    */
                    this.lastRoundBet = maxMoneyPerPlayer;
                }
            }

            return this.lastRoundBet + this.lastStepBet;
        }
    }
}
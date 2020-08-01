namespace TexasHoldem.Logic.GameMechanics
{
    public class MinRaise
    {
        private readonly int smallBlind;

        private int lastMaxMoneyPerPlayer;

        private int step;

        public MinRaise(int smallBlind)
        {
            this.smallBlind = smallBlind;
            this.Reset();
        }

        public string AggressorName { get; private set; }

        public int Amount(string currentPlayerName)
        {
            if (this.AggressorName == currentPlayerName)
            {
                /*
                * For example, we are sitting on UTG
                * first betting round:  SB->5; BB->10; UTG->raise 35; MP->re-raise all-in 37; CO->call 37
                * second betting round: SB->fold; BB->fold; UTG->only fold or call;
                * In the first betting round we were the aggressor.
                * In the second betting round, we are still the aggressor and can not raise ourselves
                */

                return 0;
            }

            return this.step;
        }

        public void Reset()
        {
            this.step = 2 * this.smallBlind;
            this.lastMaxMoneyPerPlayer = 0;
            this.AggressorName = string.Empty;
        }

        public void Update(string currentPlayerName, int maxMoneyPerPlayer, int currentRoundBet)
        {
            if (maxMoneyPerPlayer == 0)
            {
                // Start flop/turn/river. Players did not bet.
                if (currentRoundBet > this.step)
                {
                    // The size of the bet is sufficient to be considered a bet
                    this.step = currentRoundBet;
                    this.lastMaxMoneyPerPlayer = currentRoundBet;
                    this.AggressorName = currentPlayerName;
                }
                else
                {
                    this.lastMaxMoneyPerPlayer = currentRoundBet;
                }
            }
            else
            {
                if (currentRoundBet >= maxMoneyPerPlayer + this.step)
                {
                    // The size of the bet/raise is sufficient to be considered a bet/raise
                    this.step = currentRoundBet - maxMoneyPerPlayer;
                    this.lastMaxMoneyPerPlayer = currentRoundBet;
                    this.AggressorName = currentPlayerName;
                }
                else
                {
                    /*
                     * For example, we are sitting on MP
                     * SB->5; BB->10; UTG->raise 35; MP->re-raise all-in 37
                     * Since we did not raise enough (37 instead of 60), the aggressor remains the same (UTG)
                    */

                    if (currentRoundBet > this.lastMaxMoneyPerPlayer)
                    {
                        this.lastMaxMoneyPerPlayer = currentRoundBet;
                    }
                }
            }
        }
    }
}

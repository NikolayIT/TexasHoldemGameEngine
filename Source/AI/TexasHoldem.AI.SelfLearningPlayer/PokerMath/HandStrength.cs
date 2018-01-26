namespace TexasHoldem.AI.SelfLearningPlayer.PokerMath
{
    public struct HandStrength
    {
        public HandStrength(Pocket pocket, double equity)
        {
            this.Pocket = pocket;
            this.Equity = equity;
        }

        public Pocket Pocket { get; }

        public double Equity { get; }
    }
}

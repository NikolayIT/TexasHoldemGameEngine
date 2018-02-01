namespace TexasHoldem.AI.SelfLearningPlayer.PokerMath
{
    public struct HandStrength
    {
        public HandStrength(IPocket pocket, double equity)
        {
            this.Pocket = pocket;
            this.Equity = equity;
        }

        public IPocket Pocket { get; }

        public double Equity { get; }
    }
}

namespace TexasHoldem.Logic.Players
{
    internal class InternalPlayer : PlayerDecorator
    {
        public InternalPlayer(IPlayer player, int initialMoney)
            : base(player)
        {
            this.Money = initialMoney;
        }

        public int Money { get; set; }

        public int CurrentBet { get; private set; }

        public int CurrentlyInPot { get; private set; }

        public override void StartHand(StartHandContext context)
        {
            this.CurrentlyInPot = 0;
            base.StartHand(context);
        }

        public override void EndRound()
        {
            this.CurrentBet = 0;
            base.EndRound();
        }

        public void Bet(int money)
        {
            this.CurrentBet += money;
            this.CurrentlyInPot += money;
            this.Money -= money;
        }
    }
}

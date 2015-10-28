namespace TexasHoldem.Logic.GameMechanics
{
    using TexasHoldem.Logic.Players;

    internal class InternalPlayer : PlayerDecorator
    {
        public InternalPlayer(IPlayer player)
            : base(player)
        {
        }

        public int Money { get; set; }

        public int CurrentBet { get; private set; }

        public int CurrentlyInPot { get; private set; }

        public bool InHand { get; set; }

        public bool ShouldPlayInRound { get; set; }

        public override void StartGame(StartGameContext context)
        {
            this.Money = context.StartMoney;
            base.StartGame(context);
        }

        public override void StartHand(StartHandContext context)
        {
            this.CurrentlyInPot = 0;
            this.InHand = true;
            base.StartHand(context);
        }

        public override void StartRound(StartRoundContext context)
        {
            if (this.InHand)
            {
                this.ShouldPlayInRound = true;
            }

            base.StartRound(context);
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

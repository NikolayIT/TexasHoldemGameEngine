namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Players;

    internal class InternalPlayer : PlayerDecorator
    {
        public InternalPlayer(IPlayer player)
            : base(player)
        {
            this.Cards = new List<Card>();
        }

        public List<Card> Cards { get; }

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
            this.Cards.Clear();
            this.Cards.Add(context.FirstCard);
            this.Cards.Add(context.SecondCard);

            this.CurrentlyInPot = 0;
            this.CurrentBet = 0;
            this.InHand = true;

            base.StartHand(context);
        }

        public override void StartRound(StartRoundContext context)
        {
            this.CurrentBet = 0;
            if (this.InHand)
            {
                this.ShouldPlayInRound = true;
            }

            base.StartRound(context);
        }

        // TODO: Extract next methods in separate MoneyManager class for easier testing?
        public void Call(int maxMoneyPerPlayer)
        {
            var diff = maxMoneyPerPlayer - this.CurrentBet;
            this.PlaceMoney(diff);
        }

        public void PlaceMoney(int money)
        {
            this.CurrentBet += money;
            this.CurrentlyInPot += money;
            this.Money -= money;
        }
    }
}

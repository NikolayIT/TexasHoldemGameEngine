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

        public InternalPlayerMoney PlayerMoney { get; private set; }

        public override void StartGame(IStartGameContext context)
        {
            this.PlayerMoney = new InternalPlayerMoney(context.StartMoney);
            base.StartGame(context);
        }

        public override void StartHand(IStartHandContext context)
        {
            this.Cards.Clear();
            this.Cards.Add(context.FirstCard);
            this.Cards.Add(context.SecondCard);

            this.PlayerMoney.NewHand();

            base.StartHand(context);
        }

        public override void StartRound(IStartRoundContext context)
        {
            this.PlayerMoney.NewRound();
            base.StartRound(context);
        }
    }
}

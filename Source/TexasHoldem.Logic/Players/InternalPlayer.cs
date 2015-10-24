namespace TexasHoldem.Logic.Players
{
    public class InternalPlayer : PlayerDecorator
    {
        public InternalPlayer(IPlayer player, int initialMoney)
            : base(player)
        {
            this.Money = initialMoney;
        }

        public int Money { get; set; }
    }
}

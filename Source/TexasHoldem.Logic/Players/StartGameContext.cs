namespace TexasHoldem.Logic.Players
{
    public class StartGameContext
    {
        public StartGameContext(int startMoney)
        {
            this.StartMoney = startMoney;
        }

        public int StartMoney { get; }
    }
}

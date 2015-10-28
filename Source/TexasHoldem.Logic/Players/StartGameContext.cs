namespace TexasHoldem.Logic.Players
{
    using System.Collections.Generic;

    public class StartGameContext
    {
        public StartGameContext(IReadOnlyCollection<string> otherPlayerNames, int startMoney)
        {
            this.OtherPlayerNames = otherPlayerNames;
            this.StartMoney = startMoney;
        }

        public IReadOnlyCollection<string> OtherPlayerNames { get; set; }

        public int StartMoney { get; }
    }
}

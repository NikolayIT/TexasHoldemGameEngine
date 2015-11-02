namespace TexasHoldem.Logic.Players
{
    using System.Collections.Generic;

    public class StartGameContext
    {
        public StartGameContext(IReadOnlyCollection<string> playerNames, int startMoney)
        {
            this.PlayerNames = playerNames;
            this.StartMoney = startMoney;
        }

        public IReadOnlyCollection<string> PlayerNames { get; }

        public int StartMoney { get; }
    }
}

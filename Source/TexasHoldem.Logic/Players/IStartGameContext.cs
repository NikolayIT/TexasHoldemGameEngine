namespace TexasHoldem.Logic.Players
{
    using System.Collections.Generic;

    public interface IStartGameContext
    {
        IReadOnlyCollection<string> PlayerNames { get; }
        int StartMoney { get; }
    }
}
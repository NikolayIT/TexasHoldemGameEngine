using System.Collections.Generic;

namespace TexasHoldem.Logic.Players
{
    public interface IStartGameContext
    {
        IReadOnlyCollection<string> PlayerNames { get; }
        int StartMoney { get; }
    }
}
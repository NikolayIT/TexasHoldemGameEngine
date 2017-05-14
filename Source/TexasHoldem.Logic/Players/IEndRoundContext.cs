using System.Collections.Generic;

namespace TexasHoldem.Logic.Players
{
    public interface IEndRoundContext
    {
        IReadOnlyCollection<PlayerActionAndName> RoundActions { get; }
    }
}
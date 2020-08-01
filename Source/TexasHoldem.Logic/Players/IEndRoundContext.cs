namespace TexasHoldem.Logic.Players
{
    using System.Collections.Generic;

    public interface IEndRoundContext
    {
        IReadOnlyCollection<PlayerActionAndName> RoundActions { get; }
    }
}

namespace TexasHoldem.Logic.Players
{
    using System.Collections.Generic;

    public class EndRoundContext
    {
        public EndRoundContext(IReadOnlyCollection<PlayerActionAndName> roundActions)
        {
            this.RoundActions = roundActions;
        }

        public IReadOnlyCollection<PlayerActionAndName> RoundActions { get; }
    }
}

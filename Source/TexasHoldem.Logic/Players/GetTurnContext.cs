namespace TexasHoldem.Logic.Players
{
    using System.Collections.Generic;

    public class GetTurnContext
    {
        public GetTurnContext(
            GameRoundType roundType,
            IReadOnlyCollection<PlayerActionAndName> previousActions,
            int smallBlind,
            int currentPot,
            int maxMoneyPerPlayer)
        {
            this.RoundType = roundType;
            this.PreviousActions = previousActions;
            this.SmallBlind = smallBlind;
            this.CurrentPot = currentPot;
            this.MaxMoneyPerPlayer = maxMoneyPerPlayer;
        }

        public GameRoundType RoundType { get; }

        public IReadOnlyCollection<PlayerActionAndName> PreviousActions { get; }

        public int SmallBlind { get; }

        public int CurrentPot { get; }

        public int MaxMoneyPerPlayer { get; }
    }
}

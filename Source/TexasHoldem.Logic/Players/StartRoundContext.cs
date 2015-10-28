namespace TexasHoldem.Logic.Players
{
    public class StartRoundContext
    {
        public StartRoundContext(GameRoundType roundType)
        {
            this.RoundType = roundType;
        }

        public GameRoundType RoundType { get; }
    }
}

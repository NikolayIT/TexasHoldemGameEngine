namespace TexasHoldem.AI.SelfLearningPlayer.Strategy
{
    public interface IPlayingStyle
    {
        double VPIP { get; }

        double PFR { get; }

        double PreflopThreeBet { get; }
    }
}

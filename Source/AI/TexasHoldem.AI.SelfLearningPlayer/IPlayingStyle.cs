namespace TexasHoldem.AI.SelfLearningPlayer
{
    public interface IPlayingStyle
    {
        double VPIP { get; }

        double PFR { get; }
    }
}

namespace TexasHoldem.Logic.Players
{
    public interface IPostingBlindContext
    {
        PlayerAction BlindAction { get; }

        int CurrentStackSize { get; }

        int CurrentPot { get; }
    }
}

namespace TexasHoldem.Logic.Players
{
    public class PostingBlindContext : IPostingBlindContext
    {
        public PostingBlindContext(PlayerAction blindAction, int currentPot, int currentStackSize)
        {
            this.BlindAction = blindAction;
            this.CurrentPot = currentPot;
            this.CurrentStackSize = currentStackSize;
        }

        public PlayerAction BlindAction { get; }

        public int CurrentPot { get; }

        public int CurrentStackSize { get; }
    }
}

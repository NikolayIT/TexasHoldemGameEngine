namespace TexasHoldem.Logic.Players
{
    public class EndGameContext
    {
        public EndGameContext(string winnerName)
        {
            this.WinnerName = winnerName;
        }

        public string WinnerName { get; }
    }
}

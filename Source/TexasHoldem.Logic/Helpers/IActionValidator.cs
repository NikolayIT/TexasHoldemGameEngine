namespace TexasHoldem.Logic.Helpers
{
    using TexasHoldem.Logic.Players;

    public interface IActionValidator
    {
        bool IsValid(GetTurnContext context);
    }
}

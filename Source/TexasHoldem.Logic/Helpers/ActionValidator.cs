namespace TexasHoldem.Logic.Helpers
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.Players;

    public class ActionValidator : IActionValidator
    {
        public bool IsValid(ICollection<PlayerActionAndName> previousActions, PlayerActionAndName action, int playerMoney)
        {
            return true;
        }
    }
}

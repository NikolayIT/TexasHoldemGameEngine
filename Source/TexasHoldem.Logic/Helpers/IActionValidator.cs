namespace TexasHoldem.Logic.Helpers
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.Players;

    public interface IActionValidator
    {
        bool IsValid(ICollection<PlayerActionAndName> previousActions, PlayerActionAndName action, int playerMoney);
    }
}

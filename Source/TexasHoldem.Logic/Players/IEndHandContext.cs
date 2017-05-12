using System.Collections.Generic;
using TexasHoldem.Logic.Cards;

namespace TexasHoldem.Logic.Players
{
    public interface IEndHandContext
    {
        Dictionary<string, ICollection<Card>> ShowdownCards { get; }
    }
}
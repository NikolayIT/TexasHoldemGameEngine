namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;

    using Cards;
    using TexasHoldem.Logic.Players;

    public interface IInternalPlayer : IPlayer
    {
        List<Card> Cards { get; }

        InternalPlayerMoney PlayerMoney { get; }
    }
}

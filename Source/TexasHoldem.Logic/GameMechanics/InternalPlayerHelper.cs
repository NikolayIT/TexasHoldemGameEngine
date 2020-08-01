namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;
    using System.Linq;

    internal static class InternalPlayerHelper
    {
        public static IEnumerable<InternalPlayer> WithMoney(this IEnumerable<InternalPlayer> players) =>
            players.Where(p => p.PlayerMoney.Money > 0);

        public static IEnumerable<InternalPlayer> InThisHand(this IEnumerable<InternalPlayer> players) =>
            players.Where(p => p.PlayerMoney.InHand);
    }
}

namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.Players;

    public class HandLogic
    {
        private readonly IList<IPlayer> players;
        private int firstToPlay;

        public HandLogic(IList<IPlayer> players, int roundNumber)
        {
            this.players = players;

            // TODO: Improve logic for selecting first player. What happens when one player drops
            this.firstToPlay = (roundNumber - 1) % this.players.Count;
        }

        public void Play()
        {
        }
    }
}

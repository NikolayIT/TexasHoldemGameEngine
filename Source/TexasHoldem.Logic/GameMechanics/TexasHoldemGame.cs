namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.Players;

    public class TexasHoldemGame : ITexasHoldemGame
    {
        private readonly IList<IPlayer> players;

        private int roundNumber = 0;

        public TexasHoldemGame(IList<IPlayer> players)
        {
            this.players = players;
        }

        public void Start()
        {
            while (this.AtLeastTwoPlayersHaveMoney())
            {
                this.roundNumber++;
                var hand = new HandLogic(this.players, this.roundNumber);
                hand.Play();
            }
        }

        private bool AtLeastTwoPlayersHaveMoney()
        {
            return true;
        }
    }
}

namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Players;

    public class TexasHoldemGame : ITexasHoldemGame
    {
        private readonly IList<InternalPlayer> players;

        private int roundNumber = 0;

        public TexasHoldemGame(ICollection<IPlayer> players, int initialMoney = 1000)
        {
            this.players = new List<InternalPlayer>(players.Count);
            foreach (var player in players)
            {
                this.players.Add(new InternalPlayer(player, initialMoney));
            }
        }

        public void Start()
        {
            while (this.AtLeastTwoPlayersHaveMoney())
            {
                this.roundNumber++;
                var smallBlind = (((this.roundNumber - 1) / 10) + 1) * 2;
                var hand = new HandLogic(this.players, this.roundNumber, smallBlind);
                hand.Play();
            }
        }

        private bool AtLeastTwoPlayersHaveMoney()
        {
            var playersWithMoney = this.players.Count(x => x.Money > 0);
            return playersWithMoney >= 2;
        }
    }
}

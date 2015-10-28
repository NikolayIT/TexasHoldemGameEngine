namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Players;

    public class TwoPlayersTexasHoldemGame : ITexasHoldemGame
    {
        private readonly InternalPlayer firstPlayer;

        private readonly InternalPlayer secondPlayer;

        private int roundNumber;

        public TwoPlayersTexasHoldemGame(IPlayer firstPlayer, IPlayer secondPlayer, int initialMoney = 1000)
        {
            this.firstPlayer = new InternalPlayer(firstPlayer, initialMoney);
            this.secondPlayer = new InternalPlayer(secondPlayer, initialMoney);
            this.roundNumber = 0;
        }

        public void Start()
        {
            // TODO: Pass initial money
            this.firstPlayer.StartGame();
            this.secondPlayer.StartGame();

            while (this.AllPlayersHaveMoney())
            {
                this.roundNumber++;
                var smallBlind = (((this.roundNumber - 1) / 10) + 1) * 2;
                var hand = this.roundNumber % 2 == 1
                               ? new TwoPlayersHandLogic(this.firstPlayer, this.secondPlayer, smallBlind)
                               : new TwoPlayersHandLogic(this.secondPlayer, this.firstPlayer, smallBlind);
                hand.Play();
            }

            this.firstPlayer.EndGame();
            this.secondPlayer.EndGame();
        }

        private bool AllPlayersHaveMoney()
        {
            return this.firstPlayer.Money > 0 && this.secondPlayer.Money > 0;
        }
    }
}

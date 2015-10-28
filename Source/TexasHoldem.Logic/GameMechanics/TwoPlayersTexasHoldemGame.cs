namespace TexasHoldem.Logic.GameMechanics
{
    using System;

    using TexasHoldem.Logic.Players;

    public class TwoPlayersTexasHoldemGame : ITexasHoldemGame
    {
        private readonly InternalPlayer firstPlayer;

        private readonly InternalPlayer secondPlayer;

        private readonly int initialMoney;

        private int roundNumber;

        public TwoPlayersTexasHoldemGame(IPlayer firstPlayer, IPlayer secondPlayer, int initialMoney = 1000)
        {
            if (firstPlayer == null)
            {
                throw new ArgumentNullException(nameof(firstPlayer));
            }

            if (secondPlayer == null)
            {
                throw new ArgumentNullException(nameof(secondPlayer));
            }

            if (initialMoney <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initialMoney), "Initial money should be greater than 0");
            }

            // Ensure the players have unique names
            if (firstPlayer.Name == secondPlayer.Name)
            {
                throw new ArgumentException($"Both players have the same name: \"{firstPlayer.Name}\"");
            }

            this.firstPlayer = new InternalPlayer(firstPlayer, initialMoney);
            this.secondPlayer = new InternalPlayer(secondPlayer, initialMoney);
            this.initialMoney = initialMoney;
            this.roundNumber = 0;
        }

        public void Start()
        {
            this.firstPlayer.StartGame(new StartGameContext(this.initialMoney));
            this.secondPlayer.StartGame(new StartGameContext(this.initialMoney));

            while (this.AllPlayersHaveMoney())
            {
                this.roundNumber++;
                var smallBlind = (((this.roundNumber - 1) / 10) + 1) * 2;
                var hand = this.roundNumber % 2 == 1
                               ? new TwoPlayersHandLogic(this.firstPlayer, this.secondPlayer, smallBlind)
                               : new TwoPlayersHandLogic(this.secondPlayer, this.firstPlayer, smallBlind);
                hand.Play();
            }

            this.firstPlayer.EndGame(new EndGameContext());
            this.secondPlayer.EndGame(new EndGameContext());
        }

        private bool AllPlayersHaveMoney()
        {
            return this.firstPlayer.Money > 0 && this.secondPlayer.Money > 0;
        }
    }
}

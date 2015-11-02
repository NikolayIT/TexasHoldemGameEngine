namespace TexasHoldem.Logic.GameMechanics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Players;

    public class TwoPlayersTexasHoldemGame : ITexasHoldemGame
    {
        private static readonly int[] SmallBlinds =
            {
                1, 2, 3, 5, 10, 15, 20, 25, 30, 40, 50, 60, 80, 100, 150, 200, 300,
                400, 500, 600, 800, 1000, 1500, 2000, 3000, 4000, 5000, 6000, 8000,
                10000, 15000, 20000, 30000, 40000, 50000, 60000, 80000, 100000
            };

        private readonly InternalPlayer firstPlayer;

        private readonly InternalPlayer secondPlayer;

        private readonly ICollection<InternalPlayer> allPlayers;

        private readonly int initialMoney;

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

            if (initialMoney <= 0 || initialMoney > 200000)
            {
                throw new ArgumentOutOfRangeException(nameof(initialMoney), "Initial money should be greater than 0 and less than 200000");
            }

            // Ensure the players have unique names
            if (firstPlayer.Name == secondPlayer.Name)
            {
                throw new ArgumentException($"Both players have the same name: \"{firstPlayer.Name}\"");
            }

            this.firstPlayer = new InternalPlayer(firstPlayer);
            this.secondPlayer = new InternalPlayer(secondPlayer);
            this.allPlayers = new List<InternalPlayer> { this.firstPlayer, this.secondPlayer };
            this.initialMoney = initialMoney;
            this.HandsPlayed = 0;
        }

        public int HandsPlayed { get; private set; }

        public IPlayer Start()
        {
            var playerNames = this.allPlayers.Select(x => x.Name).ToList().AsReadOnly();
            foreach (var player in this.allPlayers)
            {
                player.StartGame(new StartGameContext(playerNames, this.initialMoney));
            }

            // While at least two players have money
            while (this.allPlayers.Count(x => x.PlayerMoney.Money > 0) > 1)
            {
                this.HandsPlayed++;

                // Every 10 hands the blind increases
                var smallBlind = SmallBlinds[(this.HandsPlayed - 1) / 10];

                // Rotate players
                var hand = this.HandsPlayed % 2 == 1
                               ? new TwoPlayersHandLogic(new[] { this.firstPlayer, this.secondPlayer }, this.HandsPlayed, smallBlind)
                               : new TwoPlayersHandLogic(new[] { this.secondPlayer, this.firstPlayer }, this.HandsPlayed, smallBlind);

                hand.Play();
            }

            var winner = this.allPlayers.FirstOrDefault(x => x.PlayerMoney.Money > 0);
            foreach (var player in this.allPlayers)
            {
                player.EndGame(new EndGameContext(winner.Name));
            }

            return winner;
        }
    }
}

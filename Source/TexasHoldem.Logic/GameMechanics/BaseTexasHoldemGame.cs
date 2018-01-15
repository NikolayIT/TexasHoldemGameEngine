namespace TexasHoldem.Logic.GameMechanics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Players;

    public abstract class BaseTexasHoldemGame : ITexasHoldemGame
    {
        protected static readonly int[] SmallBlinds =
            {
                10, 2, 3, 5, 10, 15, 20, 25, 30, 40, 50, 60, 80, 100, 150, 200, 300,
                400, 500, 600, 800, 1000, 1500, 2000, 3000, 4000, 5000, 6000, 8000,
                10000, 15000, 20000, 30000, 40000, 50000, 60000, 80000, 100000
            };

        private readonly ICollection<IInternalPlayer> allPlayers;

        private int initialMoney;

        public BaseTexasHoldemGame(ICollection<IPlayer> allPlayers, int initialMoney = 1000)
        {
            if (allPlayers == null)
            {
                throw new ArgumentNullException(nameof(allPlayers));
            }

            if (allPlayers.Count < 2 || allPlayers.Count > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(allPlayers), "The number of players must be from 2 to 10");
            }

            if (initialMoney <= 0 || initialMoney > 200000)
            {
                throw new ArgumentOutOfRangeException(nameof(initialMoney), "Initial money should be greater than 0 and less than 200000");
            }

            this.allPlayers = new List<IInternalPlayer>(allPlayers.Count);
            foreach (var item in allPlayers)
            {
                this.allPlayers.Add(new InternalPlayer(item));
            }

            this.initialMoney = initialMoney;
            this.HandsPlayed = 0;
        }

        public int HandsPlayed { get; protected set; }

        protected IReadOnlyList<IInternalPlayer> AllPlayers
        {
            get
            {
                return this.allPlayers.ToList();
            }
        }

        public abstract void PlayHand();

        public IPlayer Start()
        {
            var playerNames = this.allPlayers.Select(x => x.Name).ToList().AsReadOnly();
            foreach (var player in this.allPlayers)
            {
                player.StartGame(new StartGameContext(playerNames, this.initialMoney));
            }

            this.PlayHand();

            var winner = this.allPlayers.FirstOrDefault(x => x.PlayerMoney.Money > 0);
            foreach (var player in this.allPlayers)
            {
                player.EndGame(new EndGameContext(winner.Name));
            }

            return winner;
        }
    }
}
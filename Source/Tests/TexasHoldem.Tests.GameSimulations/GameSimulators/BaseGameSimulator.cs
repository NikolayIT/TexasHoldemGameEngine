namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using System;
    using System.Diagnostics;

    using TexasHoldem.Logic.GameMechanics;
    using TexasHoldem.Logic.Players;

    public abstract class BaseGameSimulator : IGameSimulator
    {
        public GameSimulationResult Simulate(int numberOfGames)
        {
            var firstPlayer = this.GetFirstPlayer();
            var secondPlayer = this.GetSecondPlayer();

            var stopwatch = Stopwatch.StartNew();

            var pointsLock = new object();
            var firstPlayerWins = 0;
            var secondPlayerWins = 0;
            var handsPlayed = 0;

            //// TODO: Parallel.For(1, numberOfGames + 1, i =>
            for (var i = 1; i < numberOfGames + 1; i++)
            {
                if (i % 100 == 0)
                {
                    Console.Write(".");
                }

                ITexasHoldemGame game = i % 2 == 1
                                            ? new TwoPlayersTexasHoldemGame(firstPlayer, secondPlayer)
                                            : new TwoPlayersTexasHoldemGame(secondPlayer, firstPlayer);

                var winner = game.Start();

                lock (pointsLock)
                {
                    if (winner.Name == firstPlayer.Name)
                    {
                        firstPlayerWins++;
                    }
                    else
                    {
                        secondPlayerWins++;
                    }

                    handsPlayed += game.HandsPlayed;
                }
            }

            var simulationDuration = stopwatch.Elapsed;

            return new GameSimulationResult
                       {
                           FirstPlayerWins = firstPlayerWins,
                           SecondPlayerWins = secondPlayerWins,
                           HandsPlayed = handsPlayed,
                           SimulationDuration = simulationDuration
                       };
        }

        protected abstract IPlayer GetFirstPlayer();

        protected abstract IPlayer GetSecondPlayer();
    }
}

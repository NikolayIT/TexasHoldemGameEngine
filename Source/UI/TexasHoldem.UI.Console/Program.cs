namespace TexasHoldem.UI.Console
{
    using System;

    using TexasHoldem.AI.DummyPlayer;
    using TexasHoldem.AI.SelfLearningPlayer;
    using TexasHoldem.AI.SmartPlayer;
    using TexasHoldem.Logic.GameMechanics;

    public static class Program
    {
        private const string ProgramName = "TexasHoldem.UI.Console (c) 2015-2018";

        private const int GameWidth = 66;

        public static void Main()
        {
            // var game = HeadsUp();
            // var game = HumanVsDummy(4);
            // var game = HumanVsHuman(3);
            // var game = HumanVsSmart(6);
            var game = HumanVsCheater(6);

            game.Start();
        }

        private static ITexasHoldemGame HeadsUp(int opponentTypeId = 2)
        {
            Stand(13);

            var consolePlayer1 = new ConsoleUiDecorator(new ConsolePlayer(0), 0, GameWidth, 5);
            switch (opponentTypeId)
            {
                case 1:
                    return new TexasHoldemGame(consolePlayer1, new ConsoleUiDecorator(new DummyPlayer(), 7, GameWidth, 5));
                case 2:
                    return new TexasHoldemGame(consolePlayer1, new ConsoleUiDecorator(new SmartPlayer(), 7, GameWidth, 5));
                case 3:
                    return new TexasHoldemGame(consolePlayer1, new ConsoleUiDecorator(new ConsolePlayer(7, "Human_2"), 7, GameWidth, 5));
                case 4:
                    return new TexasHoldemGame(consolePlayer1, new ConsoleUiDecorator(new Cheater(new PlayingStyle()), 7, GameWidth, 5));
                default:
                    throw new Exception();
            }
        }

        private static ITexasHoldemGame MultiplePlayers(int numberOfPlayers, int opponentTypeId)
        {
            if (numberOfPlayers == 2)
            {
                return HeadsUp(opponentTypeId);
            }

            var numberOfCommonRows = 3; // Place for community cards, pot, main pot, side pots
            int gameHeight = (6 * numberOfPlayers) + numberOfCommonRows;
            Stand(gameHeight);

            ConsoleUiDecorator[] players = new ConsoleUiDecorator[numberOfPlayers];
            players[0] = new ConsoleUiDecorator(
                new ConsolePlayer(numberOfCommonRows, "Human_1", 250), numberOfCommonRows, GameWidth, 1);
            for (int i = 1; i < numberOfPlayers; i++)
            {
                switch (opponentTypeId)
                {
                    case 1:
                        players[i] = new ConsoleUiDecorator(new DummyPlayer(), (6 * i) + numberOfCommonRows, GameWidth, 1);
                        break;
                    case 2:
                        players[i] = new ConsoleUiDecorator(new SmartPlayer(), (6 * i) + numberOfCommonRows, GameWidth, 1);
                        break;
                    case 3:
                        var row = (6 * i) + numberOfCommonRows;
                        players[i] = new ConsoleUiDecorator(
                            new ConsolePlayer(row, "Human_" + i + 1, 250 - (i * 20)), row, GameWidth, 1);
                        break;
                    case 4:
                        players[i] = new ConsoleUiDecorator(new Cheater(new PlayingStyle()), (6 * i) + numberOfCommonRows, GameWidth, 1);
                        break;
                    default:
                        break;
                }
            }

            return new TexasHoldemGame(players);
        }

        private static ITexasHoldemGame HumanVsDummy(int numberOfPlayers)
        {
            return MultiplePlayers(numberOfPlayers, 1);
        }

        private static ITexasHoldemGame HumanVsSmart(int numberOfPlayers)
        {
            return MultiplePlayers(numberOfPlayers, 2);
        }

        private static ITexasHoldemGame HumanVsHuman(int numberOfPlayers)
        {
            return MultiplePlayers(numberOfPlayers, 3);
        }

        private static ITexasHoldemGame HumanVsCheater(int numberOfPlayers)
        {
            return MultiplePlayers(numberOfPlayers, 4);
        }

        private static void Stand(int gameHeight)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BufferHeight = Console.WindowHeight = gameHeight;
            Console.BufferWidth = Console.WindowWidth = GameWidth;

            ConsoleHelper.WriteOnConsole(gameHeight - 1, GameWidth - ProgramName.Length - 1, ProgramName, ConsoleColor.Green);
        }
    }
}

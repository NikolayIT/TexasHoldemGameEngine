namespace TexasHoldem.UI.Console
{
    using System;

    using TexasHoldem.AI.DummyPlayer;
    using TexasHoldem.AI.SmartPlayer;
    using TexasHoldem.Logic.GameMechanics;

    public static class Program
    {
        private const string ProgramName = "TexasHoldem.UI.Console (c) 2015-2018";

        private const int GameWidth = 66;

        public static void Main()
        {
            // HeadsUp();
            MultiplePlayers(6);
        }

        private static void HeadsUp()
        {
            Stand(13);

            var consolePlayer1 = new ConsoleUiDecorator(new ConsolePlayer(0), 0, GameWidth, 5);
            var consolePlayer2 = new ConsoleUiDecorator(new SmartPlayer(), 7, GameWidth, 5);
            ITexasHoldemGame game = new TexasHoldemGame(consolePlayer1, consolePlayer2);
            game.Start();
        }

        private static void MultiplePlayers(int numberOfPlayers)
        {
            if (numberOfPlayers == 2)
            {
                HeadsUp();
                return;
            }

            var numberOfCommonRows = 3; // Place for community cards, pot, main pot, side pots
            int gameHeight = (6 * numberOfPlayers) + numberOfCommonRows;
            Stand(gameHeight);

            ConsoleUiDecorator[] players = new ConsoleUiDecorator[numberOfPlayers];
            players[0] = new ConsoleUiDecorator(
                new ConsolePlayer(numberOfCommonRows, "ConsolePlayer_1", 250), numberOfCommonRows, GameWidth, 1);
            for (int i = 1; i < numberOfPlayers; i++)
            {
                players[i] = new ConsoleUiDecorator(new DummyPlayer(), (6 * i) + numberOfCommonRows, GameWidth, 1);
                //players[i] = new ConsoleUiDecorator(
                //    new ConsolePlayer((6 * i) + numberOfCommonRows, "ConsolePlayer_" + i + 1, 250 - (i * 20)),
                //    (6 * i) + numberOfCommonRows,
                //    GameWidth,
                //    1);
            }

            ITexasHoldemGame game = new TexasHoldemGame(players);
            game.Start();
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

namespace TexasHoldem.UI.Console
{
    using System;

    using TexasHoldem.AI.SmartPlayer;
    using TexasHoldem.Logic.GameMechanics;

    public static class Program
    {
        private const string ProgramName = "TexasHoldem.UI.Console (c) 2015-2018";

        public static void Main()
        {
            // HeadsUp(12, 66);
            MultiplePlayers(36, 66);
        }

        private static void HeadsUp(int gameHeight, int gameWidth)
        {
            Stand(gameHeight, gameWidth);

            var consolePlayer1 = new ConsoleUiDecorator(new ConsolePlayer(0), 0, gameWidth, 5);
            var consolePlayer2 = new ConsoleUiDecorator(new SmartPlayer(), 6, gameWidth, 5);
            ITexasHoldemGame game = new TexasHoldemGame(consolePlayer1, consolePlayer2);
            game.Start();
        }

        private static void MultiplePlayers(int gameHeight, int gameWidth)
        {
            Stand(gameHeight, gameWidth);

            var consolePlayer1 = new ConsoleUiDecorator(new ConsolePlayer(0, "ConsolePlayer_1", 90), 0, gameWidth, 5);
            var consolePlayer2 = new ConsoleUiDecorator(new ConsolePlayer(6, "ConsolePlayer_2"), 6, gameWidth, 5);
            var consolePlayer3 = new ConsoleUiDecorator(new ConsolePlayer(12, "ConsolePlayer_3", 40), 12, gameWidth, 5);
            var consolePlayer4 = new ConsoleUiDecorator(new ConsolePlayer(18, "ConsolePlayer_4", 30), 18, gameWidth, 5);
            var consolePlayer5 = new ConsoleUiDecorator(new ConsolePlayer(24, "ConsolePlayer_5"), 24, gameWidth, 5);
            var consolePlayer6 = new ConsoleUiDecorator(new ConsolePlayer(30, "ConsolePlayer_6", 60), 30, gameWidth, 5);
            ITexasHoldemGame game = new TexasHoldemGame(
                new[] { consolePlayer1, consolePlayer2, consolePlayer3, consolePlayer4, consolePlayer5, consolePlayer6, });
            game.Start();
        }

        private static void Stand(int gameHeight, int gameWidth)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BufferHeight = Console.WindowHeight = gameHeight;
            Console.BufferWidth = Console.WindowWidth = gameWidth;

            ConsoleHelper.WriteOnConsole(gameHeight - 1, gameWidth - ProgramName.Length - 1, ProgramName, ConsoleColor.Green);
        }
    }
}

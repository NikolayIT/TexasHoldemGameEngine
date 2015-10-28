namespace TexasHoldem.UI.Console
{
    using System;

    using TexasHoldem.Logic.GameMechanics;
    using TexasHoldem.Logic.Players;

    public static class Program
    {
        private const string ProgramName = "TexasHoldem.UI.Console (c) 2015";
        private const int GameHeight = 12;
        private const int GameWidth = 60;

        public static void Main()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BufferHeight = Console.WindowHeight = GameHeight;
            Console.BufferWidth = Console.WindowWidth = GameWidth;

            ConsoleHelper.WriteOnConsole(GameHeight - 1, GameWidth - ProgramName.Length - 1, ProgramName, ConsoleColor.Green);

            var consolePlayer1 = new ConsolePlayer(0, GameWidth, 5);
            var consolePlayer2 = new ConsolePlayer(6, GameWidth, 5);
            ITexasHoldemGame game = new TwoPlayersTexasHoldemGame(consolePlayer1, consolePlayer2);
            game.Start();
        }
    }
}

namespace TexasHoldem.UI.Console
{
    using System;

    using TexasHoldem.Logic.GameMechanics;
    using TexasHoldem.Logic.Players;

    public static class Program
    {
        public static void Main()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BufferHeight = Console.WindowHeight = 17;
            Console.BufferWidth = Console.WindowWidth = 50;

            var consolePlayer1 = new ConsolePlayer(1);
            var consolePlayer2 = new ConsolePlayer(10);
            ITexasHoldemGame game = new TwoPlayersTexasHoldemGame(consolePlayer1, consolePlayer2);
            game.Start();
        }
    }
}

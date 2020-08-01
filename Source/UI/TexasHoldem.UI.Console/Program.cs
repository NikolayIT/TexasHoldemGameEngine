namespace TexasHoldem.UI.Console
{
    using System;
    using System.Collections.Generic;

    using TexasHoldem.AI.DummyPlayer;
    using TexasHoldem.AI.SmartPlayer;
    using TexasHoldem.Logic.GameMechanics;
    using TexasHoldem.Logic.Players;

    public static class Program
    {
        private const string ProgramName = "TexasHoldem.UI.Console (c) 2015-2020";

        private const int GameWidth = 66;

        private const int NumberOfCommonRows = 3; // place for community cards, pot, main pot, side pots

        private static readonly List<IPlayer> Players = new List<IPlayer>();

        public static void Main()
        {
            Players.Add(new DummyPlayer());
            Players.Add(new SmartPlayer());
            Players.Add(new ConsolePlayer((6 * Players.Count) + NumberOfCommonRows));
            Players.Add(new DummyPlayer());
            Players.Add(new SmartPlayer());
            Players.Add(new DummyPlayer());

            var gameHeight = (6 * Players.Count) + NumberOfCommonRows;
            Table(gameHeight);

            var game = Game();
            game.Start();
        }

        private static ITexasHoldemGame Game()
        {
            var list = new List<IPlayer>();

            for (int i = 0; i < Players.Count; i++)
            {
                list.Add(new ConsoleUiDecorator(Players[i], (6 * i) + NumberOfCommonRows, GameWidth, 1));
            }

            return new TexasHoldemGame(list);
        }

        private static void Table(int gameHeight)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BufferHeight = Console.WindowHeight = gameHeight;
            Console.BufferWidth = Console.WindowWidth = GameWidth;

            ConsoleHelper.WriteOnConsole(gameHeight - 1, GameWidth - ProgramName.Length - 1, ProgramName, ConsoleColor.Green);
        }
    }
}

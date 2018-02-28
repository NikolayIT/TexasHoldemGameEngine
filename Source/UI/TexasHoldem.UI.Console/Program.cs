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
        private const string ProgramName = "TexasHoldem.UI.Console (c) 2015-2018";

        private const int GameWidth = 66;

        private const int NumberOfCommonRows = 3; // place for community cards, pot, main pot, side pots

        private static List<IPlayer> players = new List<IPlayer>();

        public static void Main()
        {
            players.Add(new DummyPlayer());
            players.Add(new SmartPlayer());
            players.Add(new ConsolePlayer((6 * players.Count) + NumberOfCommonRows));
            players.Add(new DummyPlayer());
            players.Add(new SmartPlayer());
            players.Add(new DummyPlayer());

            var gameHeight = (6 * players.Count) + NumberOfCommonRows;
            Table(gameHeight);

            var game = Game();
            game.Start();
        }

        private static ITexasHoldemGame Game()
        {
            var list = new List<IPlayer>();

            for (int i = 0; i < players.Count; i++)
            {
                list.Add(new ConsoleUiDecorator(players[i], (6 * i) + NumberOfCommonRows, GameWidth, 1));
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
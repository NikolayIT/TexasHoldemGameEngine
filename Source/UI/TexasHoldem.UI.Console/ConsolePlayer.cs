namespace TexasHoldem.UI.Console
{
    using System;

    using TexasHoldem.Logic.Players;

    public class ConsolePlayer : BasePlayer
    {
        private readonly int row;

        public ConsolePlayer(int row, string name = "Console Player")
        {
            this.row = row;
            this.Name = name;
        }

        public override string Name { get; }

        public override void StartHand(StartHandContext context)
        {
            WriteOnConsole(this.row, 1, context.FirstCard + "  ");
            WriteOnConsole(this.row, 6, context.SecondCard + "  ");
        }

        public override PlayerTurn GetTurn(GetTurnContext context)
        {
            WriteOnConsole(this.row + 1, 1, "Select action [C]heck/[C]all, [R]aise, [F]old");
            Console.ReadLine();
            return PlayerTurn.Fold();
        }

        private static void WriteOnConsole(int row, int col, string text, ConsoleColor foregroundColor = ConsoleColor.Gray, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.SetCursorPosition(col, row);
            Console.Write(text);
        }
    }
}
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
            WriteOnConsole(this.row + 1, 1, "Select action [C]heck/[C]all, [R]aise, [F]old, [A]ll-in");
            while (true)
            {
                var key = Console.ReadKey(true);
                PlayerTurn turn = null;
                if (key.Key == ConsoleKey.C)
                {
                    // TODO: Check or Call?
                    turn = PlayerTurn.Check();
                }
                else if (key.Key == ConsoleKey.R)
                {
                    // TODO: Ask the raise amount!
                    turn = PlayerTurn.Raise(10);
                }
                else if (key.Key == ConsoleKey.F)
                {
                    turn = PlayerTurn.Fold();
                }
                else if (key.Key == ConsoleKey.A)
                {
                    turn = PlayerTurn.Raise(this.MoneyLeft);
                }

                // TODO: Check if the turn is valid
                if (turn != null)
                {
                    WriteOnConsole(this.row + 1, 1, new string(' ', 59));
                    WriteOnConsole(this.row + 2, 1, turn.Type + "    ");
                    return turn;
                }
            }
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
namespace TexasHoldem.UI.Console
{
    using System;

    using TexasHoldem.Logic.Players;

    public class ConsolePlayer : BasePlayer
    {
        private readonly int row;

        public ConsolePlayer(int row)
        {
            this.row = row;
            this.Name = "ConsolePlayer";
        }

        public override string Name { get; }

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            this.DrawPlayerOptions(context.MoneyToCall);

            while (true)
            {
                var key = Console.ReadKey(true);
                PlayerAction action = null;
                switch (key.Key)
                {
                    case ConsoleKey.C:
                        action = PlayerAction.CheckOrCall();
                        break;
                    case ConsoleKey.R:
                        // ConsoleHelper.WriteOnConsole(this.row + 2, 2, $"Raise amount [1-{context.MoneyLeft}]:                                ");
                        // continue;
                        action = PlayerAction.Raise(10);
                        break;
                    case ConsoleKey.F:
                        action = PlayerAction.Fold();
                        break;
                    case ConsoleKey.A:
                        action = context.MoneyLeft > 0
                                     ? PlayerAction.Raise(context.MoneyLeft)
                                     : PlayerAction.CheckOrCall();
                        break;
                }

                if (action != null)
                {
                    return action;
                }
            }
        }

        private void DrawPlayerOptions(int moneyToCall)
        {
            var col = 2;
            ConsoleHelper.WriteOnConsole(this.row + 2, col, "Select action: [");
            col += 16;
            ConsoleHelper.WriteOnConsole(this.row + 2, col, "C", ConsoleColor.Yellow);
            col++;
            ConsoleHelper.WriteOnConsole(this.row + 2, col, "]heck/[");
            col += 7;
            ConsoleHelper.WriteOnConsole(this.row + 2, col, "C", ConsoleColor.Yellow);
            col++;

            var callString = moneyToCall <= 0 ? "]all, [" : "]all(" + moneyToCall + "), [";

            ConsoleHelper.WriteOnConsole(this.row + 2, col, callString);
            col += callString.Length;
            ConsoleHelper.WriteOnConsole(this.row + 2, col, "R", ConsoleColor.Yellow);
            col++;
            ConsoleHelper.WriteOnConsole(this.row + 2, col, "]aise, [");
            col += 8;
            ConsoleHelper.WriteOnConsole(this.row + 2, col, "F", ConsoleColor.Yellow);
            col++;
            ConsoleHelper.WriteOnConsole(this.row + 2, col, "]old, [");
            col += 7;
            ConsoleHelper.WriteOnConsole(this.row + 2, col, "A", ConsoleColor.Yellow);
            col++;
            ConsoleHelper.WriteOnConsole(this.row + 2, col, "]ll-in");
        }
    }
}

namespace TexasHoldem.UI.Console
{
    using System;

    using TexasHoldem.Logic.Players;

    public class ConsolePlayer : BasePlayer
    {
        private const ConsoleColor PlayerBoxColor = ConsoleColor.DarkGreen;

        private readonly int row;

        private readonly int width;

        public ConsolePlayer(int row, int width, int commonRow)
        {
            this.row = row;
            this.width = width;
            this.Name = "ConsolePlayerLine" + row;

            ConsoleHelper.WriteOnConsole(commonRow, 0, "Common roooowwww!");
            this.DrawGameBox();
        }

        public override string Name { get; }

        public override void StartHand(StartHandContext context)
        {
            ConsoleHelper.WriteOnConsole(this.row + 1, 2, this.MoneyLeft + "       ");
            ConsoleHelper.WriteOnConsole(this.row + 1, 10, context.FirstCard + "  ");
            ConsoleHelper.WriteOnConsole(this.row + 1, 15, context.SecondCard + "  ");
        }

        public override void StartRound(StartRoundContext context)
        {
            ConsoleHelper.WriteOnConsole(this.row + 1, this.width - 11, context.RoundType + "   ");
            base.StartRound(context);
        }

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            ConsoleHelper.WriteOnConsole(this.row + 2, 2, "Select action [C]heck/[C]all, [R]aise, [F]old, [A]ll-in");
            while (true)
            {
                var key = Console.ReadKey(true);
                PlayerAction action = null;
                switch (key.Key)
                {
                    case ConsoleKey.C:
                        // TODO: Check or Call?
                        action = PlayerAction.Check();
                        break;
                    case ConsoleKey.R:
                        // TODO: Ask the raise amount!
                        action = PlayerAction.Raise(10);
                        break;
                    case ConsoleKey.F:
                        action = PlayerAction.Fold();
                        break;
                    case ConsoleKey.A:
                        action = PlayerAction.Raise(this.MoneyLeft);
                        break;
                }

                // TODO: Check if the action is valid
                if (action != null)
                {
                    ConsoleHelper.WriteOnConsole(this.row + 2, 2, new string(' ', this.width - 3));
                    ConsoleHelper.WriteOnConsole(this.row + 3, 2, action + "    ");
                    return action;
                }
            }
        }

        private void DrawGameBox()
        {
            ConsoleHelper.WriteOnConsole(this.row, 0, new string('═', this.width), PlayerBoxColor);
            ConsoleHelper.WriteOnConsole(this.row + 4, 0, new string('═', this.width), PlayerBoxColor);
            ConsoleHelper.WriteOnConsole(this.row, 0, "╔", PlayerBoxColor);
            ConsoleHelper.WriteOnConsole(this.row, this.width - 1, "╗", PlayerBoxColor);
            ConsoleHelper.WriteOnConsole(this.row + 4, 0, "╚", PlayerBoxColor);
            ConsoleHelper.WriteOnConsole(this.row + 4, this.width - 1, "╝", PlayerBoxColor);
            for (var i = 1; i < 4; i++)
            {
                ConsoleHelper.WriteOnConsole(this.row + i, 0, "║", PlayerBoxColor);
                ConsoleHelper.WriteOnConsole(this.row + i, this.width - 1, "║", PlayerBoxColor);
            }
        }
    }
}
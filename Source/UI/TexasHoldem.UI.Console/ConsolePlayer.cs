namespace TexasHoldem.UI.Console
{
    using System;

    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Players;

    public class ConsolePlayer : BasePlayer
    {
        private const ConsoleColor PlayerBoxColor = ConsoleColor.DarkGreen;

        private readonly int row;

        private readonly int width;

        private readonly int commonRow;

        public ConsolePlayer(int row, int width, int commonRow)
        {
            this.row = row;
            this.width = width;
            this.commonRow = commonRow;

            this.Name = "ConsolePlayerLine" + row;

            this.DrawGameBox();
        }

        public override string Name { get; }

        public override void StartHand(StartHandContext context)
        {
            this.UpdateCommonRow(0);
            ConsoleHelper.WriteOnConsole(this.row + 1, 2, context.MoneyLeft + "       ");
            ConsoleHelper.WriteOnConsole(this.row + 1, 10, context.FirstCard + "  ");
            ConsoleHelper.WriteOnConsole(this.row + 1, 15, context.SecondCard + "  ");
        }

        public override void StartRound(StartRoundContext context)
        {
            this.UpdateCommonRow(context.CurrentPot);

            ConsoleHelper.WriteOnConsole(this.row + 1, this.width - 11, context.RoundType + "   ");
            base.StartRound(context);
        }

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            this.UpdateCommonRow(context.CurrentPot);

            ConsoleHelper.WriteOnConsole(this.row + 2, 2, "Select action [C]heck/[C]all, [R]aise, [F]old, [A]ll-in");
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
                        // TODO: Ask for the raise amount!
                        action = PlayerAction.Raise(10);
                        break;
                    case ConsoleKey.F:
                        action = PlayerAction.Fold();
                        break;
                    case ConsoleKey.A:
                        action = PlayerAction.Raise(context.MoneyLeft);
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

        private void UpdateCommonRow(int pot)
        {
            // Clear the common row
            ConsoleHelper.WriteOnConsole(this.commonRow, 0, new string(' ', this.width - 1));

            var cardsAsString = this.CommunityCards.CardsToString();
            ConsoleHelper.WriteOnConsole(this.commonRow, (this.width / 2) - (cardsAsString.Length / 2), cardsAsString);

            var potAsString = pot.ToString();
            ConsoleHelper.WriteOnConsole(this.commonRow, this.width - potAsString.Length - 2, potAsString);
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
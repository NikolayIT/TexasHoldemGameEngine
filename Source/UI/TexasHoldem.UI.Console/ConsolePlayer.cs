namespace TexasHoldem.UI.Console
{
    using System;

    using TexasHoldem.Logic.Players;

    public class ConsolePlayer : BasePlayer
    {
        private readonly int row;

        public ConsolePlayer(int row, int buyIn = -1)
        {
            this.row = row;
            this.BuyIn = buyIn;
        }

        public override string Name { get; } = "ConsolePlayer_" + Guid.NewGuid();

        public override int BuyIn { get; }

        public override PlayerAction PostingBlind(IPostingBlindContext context)
        {
            return context.BlindAction;
        }

        public override PlayerAction GetTurn(IGetTurnContext context)
        {
            if (!context.CanRaise)
            {
                this.DrawRestrictedPlayerOptions(context.MoneyToCall);
            }
            else
            {
                this.DrawPlayerOptions(context.MoneyToCall);
            }

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
                        if (!context.CanRaise)
                        {
                            continue;
                        }

                        action = PlayerAction.Raise(
                            this.RaiseAmount(context.MoneyLeft, context.MinRaise, context.MoneyToCall, context.MyMoneyInTheRound));
                        break;
                    case ConsoleKey.F:
                        action = PlayerAction.Fold();
                        break;
                    case ConsoleKey.A:
                        if (!context.CanRaise)
                        {
                            continue;
                        }

                        action = context.MoneyLeft > 0
                                     ? PlayerAction.Raise(context.MoneyLeft - context.MoneyToCall)
                                     : PlayerAction.CheckOrCall();
                        break;
                }

                if (action != null)
                {
                    return action;
                }
            }
        }

        private int RaiseAmount(int moneyLeft, int minRaise, int moneyToCall, int myMoneyInTheRound)
        {
            var wholeMinRaise = minRaise + myMoneyInTheRound + moneyToCall;
            if (wholeMinRaise >= moneyLeft + myMoneyInTheRound)
            {
                // Instant All-In
                return moneyLeft - moneyToCall;
            }

            var perfix = $"Raise amount [{wholeMinRaise}-{moneyLeft + myMoneyInTheRound}]:";

            do
            {
                ConsoleHelper.WriteOnConsole(this.row + 2, 2, new string(' ', Console.WindowWidth - 3));
                ConsoleHelper.WriteOnConsole(this.row + 2, 2, perfix);

                var text = ConsoleHelper.UserInput(this.row + 2, perfix.Length + 3);
                int result;

                if (int.TryParse(text, out result))
                {
                    if (result < wholeMinRaise)
                    {
                        return minRaise;
                    }
                    else if (result >= moneyLeft + myMoneyInTheRound)
                    {
                        // Raise All-in
                        return moneyLeft - moneyToCall;
                    }

                    return result - (myMoneyInTheRound + moneyToCall);
                }
            }
            while (true);
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

        private void DrawRestrictedPlayerOptions(int moneyToCall)
        {
            var col = 2;
            ConsoleHelper.WriteOnConsole(this.row + 2, col, "Select action: [");
            col += 16;
            ConsoleHelper.WriteOnConsole(this.row + 2, col, "C", ConsoleColor.Yellow);
            col++;

            var callString = moneyToCall <= 0 ? "]all, [" : "]all(" + moneyToCall + "), [";

            ConsoleHelper.WriteOnConsole(this.row + 2, col, callString);
            col += callString.Length;
            ConsoleHelper.WriteOnConsole(this.row + 2, col, "F", ConsoleColor.Yellow);
            col++;
            ConsoleHelper.WriteOnConsole(this.row + 2, col, "]old");
        }
    }
}

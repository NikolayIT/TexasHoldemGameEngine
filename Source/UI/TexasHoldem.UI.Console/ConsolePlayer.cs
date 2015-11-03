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
    }
}

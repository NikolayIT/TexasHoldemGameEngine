namespace TexasHoldem.UI.Console
{
    using System;

    using TexasHoldem.Logic.Players;

    public class ConsolePlayer : BasePlayer
    {
        public ConsolePlayer()
        {
            this.Name = "ConsolePlayer";
        }

        public override string Name { get; }

        public override PlayerAction GetTurn(GetTurnContext context)
        {
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
                        // TODO: Ask for the raise amount
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

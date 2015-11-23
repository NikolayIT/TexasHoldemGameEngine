namespace TexasHoldem.UI.Console
{
    using System;
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Players;

    public class ConsoleUiDecorator : PlayerDecorator
    {
        private const ConsoleColor PlayerBoxColor = ConsoleColor.DarkGreen;

        private readonly int row;

        private readonly int width;

        private readonly int commonRow;

        public ConsoleUiDecorator(IPlayer player, int row, int width, int commonRow)
            : base(player)
        {
            this.row = row;
            this.width = width;
            this.commonRow = commonRow;

            this.DrawGameBox();
        }

        private IReadOnlyCollection<Card> CommunityCards { get; set; }

        public override void StartHand(StartHandContext context)
        {
            this.UpdateCommonRow(0);
            var dealerSymbol = context.FirstPlayerName == this.Player.Name ? "D" : " ";

            ConsoleHelper.WriteOnConsole(this.row + 1, 1, dealerSymbol, ConsoleColor.Green);
            ConsoleHelper.WriteOnConsole(this.row + 3, 2, "                            ");

            ConsoleHelper.WriteOnConsole(this.row + 1, 2, context.MoneyLeft.ToString());
            this.DrawSingleCard(this.row + 1, 10, context.FirstCard);
            this.DrawSingleCard(this.row + 1, 14, context.SecondCard);

            base.StartHand(context);
        }

        public override void StartRound(StartRoundContext context)
        {
            this.CommunityCards = context.CommunityCards;
            this.UpdateCommonRow(context.CurrentPot);

            ConsoleHelper.WriteOnConsole(this.row + 1, this.width - 11, context.RoundType + "   ");
            base.StartRound(context);
        }

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            this.UpdateCommonRow(context.CurrentPot);
            ConsoleHelper.WriteOnConsole(this.row + 1, 2, context.MoneyLeft + "   ");

            var action = base.GetTurn(context);

            ConsoleHelper.WriteOnConsole(this.row + 2, 2, new string(' ', this.width - 3));

            var lastAction = action.Type + (action.Type == PlayerActionType.Fold
                ? string.Empty
                : "(" + (action.Money + ((context.MoneyToCall < 0) ? 0 : context.MoneyToCall) + ")"));

            ConsoleHelper.WriteOnConsole(this.row + 3, 2, "Last action: " + lastAction + "            ");

            var moneyAfterAction = action.Type == PlayerActionType.Fold
                ? context.MoneyLeft
                : context.MoneyLeft - action.Money - context.MoneyToCall;

            ConsoleHelper.WriteOnConsole(this.row + 1, 2, moneyAfterAction + "   ");

            return action;
        }

        private void UpdateCommonRow(int pot)
        {
            // Clear the common row
            ConsoleHelper.WriteOnConsole(this.commonRow, 0, new string(' ', this.width - 1));

            this.DrawCommunityCards();

            var potAsString = "Pot: " + pot;
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

        private void DrawCommunityCards()
        {
            if (this.CommunityCards != null)
            {
                var cardsAsString = this.CommunityCards.CardsToString();
                var cardsLength = cardsAsString.Length / 2;
                var cardsStartCol = (this.width / 2) - (cardsLength / 2);
                var cardIndex = 0;
                var spacing = 0;

                foreach (var communityCard in this.CommunityCards)
                {
                    this.DrawSingleCard(this.commonRow, cardsStartCol + (cardIndex * 4) + spacing, communityCard);
                    cardIndex++;

                    spacing += communityCard.Type == CardType.Ten ? 1 : 0;
                }
            }
        }

        private void DrawSingleCard(int row, int col, Card card)
        {
            var cardColor = this.GetCardColor(card);
            ConsoleHelper.WriteOnConsole(row, col, " " + card + " ", cardColor, ConsoleColor.White);
            ConsoleHelper.WriteOnConsole(row, col + 2 + card.ToString().Length, " ");
        }

        private ConsoleColor GetCardColor(Card card)
        {
            switch (card.Suit)
            {
                case CardSuit.Club: return ConsoleColor.DarkGreen;
                case CardSuit.Diamond: return ConsoleColor.Blue;
                case CardSuit.Heart: return ConsoleColor.Red;
                case CardSuit.Spade: return ConsoleColor.Black;
                default: throw new ArgumentException("card.Suit");
            }
        }
    }
}
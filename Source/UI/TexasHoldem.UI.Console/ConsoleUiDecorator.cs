namespace TexasHoldem.UI.Console
{
    using System;
    using System.Collections.Generic;
    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.GameMechanics;
    using TexasHoldem.Logic.Players;

    public class ConsoleUiDecorator : PlayerDecorator
    {
        private const ConsoleColor PlayerBoxColor = ConsoleColor.DarkGreen;

        private readonly int row;

        private readonly int width;

        private readonly int commonRow;

        private Card firstCard;

        private Card secondCard;

        public ConsoleUiDecorator(IPlayer player, int row, int width, int commonRow)
            : base(player)
        {
            this.row = row;
            this.width = width;
            this.commonRow = commonRow;

            this.DrawGameBox();
        }

        private IReadOnlyCollection<Card> CommunityCards { get; set; }

        public override void StartHand(IStartHandContext context)
        {
            this.UpdateCommonRow(0);
            var dealerSymbol = context.FirstPlayerName == this.Player.Name ? "D" : " ";

            ConsoleHelper.WriteOnConsole(this.row + 1, 1, dealerSymbol, ConsoleColor.Green);
            ConsoleHelper.WriteOnConsole(this.row + 3, 2, "                            ");

            ConsoleHelper.WriteOnConsole(this.row + 1, 2, context.MoneyLeft.ToString());
            this.firstCard = context.FirstCard.DeepClone();
            this.secondCard = context.SecondCard.DeepClone();
            this.DrawSingleCard(this.row + 1, 10, this.firstCard);
            this.DrawSingleCard(this.row + 1, 14, this.secondCard);

            base.StartHand(context);
        }

        public override void StartRound(IStartRoundContext context)
        {
            this.CommunityCards = context.CommunityCards;
            this.UpdateCommonRow(context.CurrentPot);

            ConsoleHelper.WriteOnConsole(this.row + 1, this.width - 11, context.RoundType + "   ");
            base.StartRound(context);
        }

        public override PlayerAction PostingBlind(IPostingBlindContext context)
        {
            this.UpdateCommonRow(context.CurrentPot);
            ConsoleHelper.WriteOnConsole(this.row + 1, 2, context.CurrentStackSize + "   ");

            var action = base.PostingBlind(context);

            ConsoleHelper.WriteOnConsole(this.row + 2, 2, new string(' ', this.width - 3));
            ConsoleHelper.WriteOnConsole(this.row + 3, 2, "Last action: " + action.Type + "(" + action.Money + ")");

            var moneyAfterAction = context.CurrentStackSize;

            ConsoleHelper.WriteOnConsole(this.row + 1, 2, moneyAfterAction + "   ");

            return action;
        }

        public override PlayerAction GetTurn(IGetTurnContext context)
        {
            this.UpdateCommonRow(context.CurrentPot);
            this.UpdateSidePots(context.MainPot, context.SidePots);
            ConsoleHelper.WriteOnConsole(this.row + 1, 2, context.MoneyLeft + "   ");

            var action = base.GetTurn(context);

            if (action.Type == PlayerActionType.Fold)
            {
                this.Eliminate(context.MoneyLeft);
                return action;
            }

            ConsoleHelper.WriteOnConsole(this.row + 2, 2, new string(' ', this.width - 3));

            var lastAction = action.Type + (action.Type == PlayerActionType.Fold
                ? string.Empty
                : "(" + (action.Money + ((context.MoneyToCall < 0) ? 0 : context.MoneyToCall) + ")"));

            ConsoleHelper.WriteOnConsole(this.row + 3, 2, new string(' ', this.width - 3));
            ConsoleHelper.WriteOnConsole(this.row + 3, 2, "Last action: " + lastAction);

            var moneyAfterAction = action.Type == PlayerActionType.Fold
                ? context.MoneyLeft
                : context.MoneyLeft - action.Money - context.MoneyToCall;

            ConsoleHelper.WriteOnConsole(this.row + 1, 2, moneyAfterAction + "   ");

            return action;
        }

        private void Eliminate(int moneyLeft)
        {
            ConsoleHelper.WriteOnConsole(this.row + 1, 2, moneyLeft + "   ");
            ConsoleHelper.WriteOnConsole(this.row + 2, 2, new string(' ', this.width - 3));
            ConsoleHelper.WriteOnConsole(this.row + 3, 2, new string(' ', this.width - 3));
            ConsoleHelper.WriteOnConsole(this.row + 3, 2, "A player is eliminated");

            this.DrawMuckedSingleCard(this.row + 1, 10, this.firstCard);
            this.DrawMuckedSingleCard(this.row + 1, 14, this.secondCard);
        }

        private void UpdateCommonRow(int pot)
        {
            // Clear the common row
            ConsoleHelper.WriteOnConsole(this.commonRow, 0, new string(' ', this.width - 1));

            this.DrawCommunityCards();

            var potAsString = "Pot: " + pot;
            ConsoleHelper.WriteOnConsole(this.commonRow, this.width - potAsString.Length - 2, potAsString);
        }

        private void UpdateSidePots(int mainPot, IReadOnlyCollection<SidePot> sidePots)
        {
            if (sidePots.Count == 0)
            {
                // Clear the side pots
                ConsoleHelper.WriteOnConsole(this.commonRow + 1, 0, new string(' ', this.width - 1));
                return;
            }

            var mainPotAsString = "Main Pot: " + mainPot;
            ConsoleHelper.WriteOnConsole(this.commonRow, 2, mainPotAsString);

            var sidePotsAsString = "Side Pots: ";
            foreach (var item in sidePots)
            {
                sidePotsAsString += "[" + item.Amount.ToString() + "]";
            }

            ConsoleHelper.WriteOnConsole(this.commonRow + 1, 2, sidePotsAsString);
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

        private void DrawMuckedSingleCard(int row, int col, Card card)
        {
            ConsoleHelper.WriteOnConsole(row, col, " " + card + " ", ConsoleColor.Gray, ConsoleColor.DarkGray);
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
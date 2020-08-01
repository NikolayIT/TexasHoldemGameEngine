namespace TexasHoldem.AI.SmartPlayer
{
    using System;

    using TexasHoldem.AI.SmartPlayer.Helpers;
    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Players;

    // TODO: This player is far far away from being smart!
    public class SmartPlayer : BasePlayer
    {
        public override string Name { get; } = "SmartPlayer_" + Guid.NewGuid();

        public override int BuyIn { get; } = -1;

        public override PlayerAction PostingBlind(IPostingBlindContext context)
        {
            return context.BlindAction;
        }

        public override PlayerAction GetTurn(IGetTurnContext context)
        {
            if (context.RoundType == GameRoundType.PreFlop)
            {
                var playHand = HandStrengthValuation.PreFlop(this.FirstCard, this.SecondCard);
                if (playHand == CardValuationType.Unplayable)
                {
                    if (context.CanCheck)
                    {
                        return PlayerAction.CheckOrCall();
                    }
                    else
                    {
                        return PlayerAction.Fold();
                    }
                }

                var isRaiseOptionAvailable = context.CanRaise;
                if (playHand == CardValuationType.Risky && isRaiseOptionAvailable)
                {
                    var factor = RandomProvider.Next(1, 4);
                    return this.RaiseOrAllIn(
                        context.MinRaise, context.CurrentMaxBet, context.MoneyLeft, context.MoneyToCall, factor);
                }

                if (playHand == CardValuationType.Recommended && isRaiseOptionAvailable)
                {
                    var factor = RandomProvider.Next(3, 6);
                    return this.RaiseOrAllIn(
                        context.MinRaise, context.CurrentMaxBet, context.MoneyLeft, context.MoneyToCall, factor);
                }

                return PlayerAction.CheckOrCall();
            }

            return PlayerAction.CheckOrCall();
        }

        private PlayerAction RaiseOrAllIn(int minRaise, int currentMaxBet, int moneyLeft, int moneyToCall, int factor)
        {
            if ((minRaise * factor) + currentMaxBet > moneyLeft)
            {
                // All-in
                return PlayerAction.Raise(moneyLeft - moneyToCall);
            }
            else
            {
                return PlayerAction.Raise(minRaise * factor);
            }
        }
    }
}

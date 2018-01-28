namespace TexasHoldem.AI.SelfLearningPlayer.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.AI.SelfLearningPlayer.PokerMath;
    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Players;

    public class Conduct
    {
        private readonly ICollection<Card> heroPocket;

        private readonly IGetTurnContext context;

        private readonly ICalculator calculator;

        private readonly IPlayingStyle playingStyle;

        public Conduct(ICollection<Card> heroPocket, IGetTurnContext context, ICalculator calculator, IPlayingStyle playingStyle)
        {
            this.heroPocket = heroPocket;
            this.context = context;
            this.calculator = calculator;
            this.playingStyle = playingStyle;
        }

        public PlayerAction OptimalAction()
        {
            if (this.context.RoundType == Logic.GameRoundType.PreFlop)
            {
                return PlayerAction.CheckOrCall();
            }
            else
            {
                bool nutHand;
                bool tie;
                var investment = this.Investment(this.context.CurrentPot, out nutHand, out tie);

                if (nutHand && tie)
                {
                    // all the players have nut hand
                    return PlayerAction.CheckOrCall();
                }
                else if (nutHand && !tie)
                {
                    // TODO: The valueBet range must be made logarithmic or exponential
                    var valueBet = RandomProvider.Next(
                        this.context.MinRaise,
                        (this.context.MoneyLeft - this.context.MoneyToCall) + 1);
                    return this.RaiseOrAllIn(valueBet);
                }
                else if (investment >= this.context.MoneyToCall + this.context.MinRaise)
                {
                    return this.RaiseOrAllIn(investment);
                }
                else if (investment < this.context.MoneyToCall)
                {
                    return PlayerAction.Fold();
                }
                else
                {
                    return PlayerAction.CheckOrCall();
                }
            }
        }

        private int Investment(int pot, out bool nutHand, out bool tie)
        {
            var equity = this.calculator.Equity();
            var heroHandStrength = equity.First(x => x.Pocket.NativeType.First().Equals(this.heroPocket.First())
                && x.Pocket.NativeType.Last().Equals(this.heroPocket.Last()));

            var query = equity.GroupBy(k => k.Equity)
                .OrderByDescending(x => x.Key);

            if (query.Count() > 1)
            {
                if (heroHandStrength.Equity == query.First().Key)
                {
                    // Hero has best hand
                    var secondEquity = query.First(x => x.Key != heroHandStrength.Equity).Key;
                    if (secondEquity == 0)
                    {
                        // Hero has absolute nut hand
                        nutHand = true;
                        tie = false;
                        return -1;
                    }
                    else
                    {
                        var investment = (pot * secondEquity) / ((1.0 - secondEquity) - secondEquity);
                        nutHand = true;
                        tie = false;
                        return (int)investment;
                    }
                }
                else
                {
                    // Hero has a weak hand
                    var investment = (pot * heroHandStrength.Equity) / (1.0 - heroHandStrength.Equity);
                    nutHand = false;
                    tie = query.First(x => x.Key == heroHandStrength.Equity).Count() > 1;
                    return (int)investment;
                }
            }
            else
            {
                // All players have the same strength hands
                nutHand = true;
                tie = true;
                return -1;
            }
        }

        private PlayerAction RaiseOrAllIn(int investment)
        {
            if (investment >= this.context.MoneyLeft - this.context.MoneyToCall)
            {
                // All-In
                return PlayerAction.Raise(this.context.MoneyLeft - this.context.MoneyToCall);
            }
            else
            {
                return PlayerAction.Raise(investment - this.context.MoneyToCall);
            }
        }
    }
}

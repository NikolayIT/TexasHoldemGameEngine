namespace TexasHoldem.AI.SelfLearningPlayer.Strategy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.AI.SelfLearningPlayer.Helpers;
    using TexasHoldem.AI.SelfLearningPlayer.PokerMath;
    using TexasHoldem.AI.SelfLearningPlayer.Statistics;
    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Players;

    public class Conduct
    {
        public static readonly IReadOnlyList<ulong> PocketsFromStrongToWeak;

        private readonly IPocket heroPocket;

        private readonly IGetTurnContext context;

        private readonly ICalculator calculator;

        private readonly IPlayingStyle playingStyle;

        private Stats stats;

        static Conduct()
        {
            var pockets = new List<ulong>();
            for (int i = 0; i < 9; i++)
            {
                foreach (var item in HoldemHand.PocketHands.Group((HoldemHand.PocketHands.GroupTypeEnum)i))
                {
                    pockets.Add(item);
                }
            }

            PocketsFromStrongToWeak = pockets.AsReadOnly();
        }

        public Conduct(IPocket heroPocket, IGetTurnContext context, ICalculator calculator, IPlayingStyle playingStyle)
        {
            this.heroPocket = heroPocket;
            this.context = context;
            this.calculator = calculator;
            this.playingStyle = playingStyle;
            this.stats = new Stats();
        }

        public PlayerAction OptimalAction()
        {
            this.stats.Update(this.context.RoundType, this.context.PreviousRoundActions);

            if (this.context.RoundType == Logic.GameRoundType.PreFlop)
            {
                var relativePosition = this.RelativePosition();
                var numberOfOpponentsInHand = this.context.Opponents.Where(x => x.InHand).Count();

                if (this.IsPlayablePocket(relativePosition, numberOfOpponentsInHand, this.playingStyle.PFR, 0.3))
                {
                    if (this.context.AvailablePlayerOptions.Contains(PlayerActionType.Raise))
                    {
                        if (this.stats.PreflopThreeBetOpportunity)
                        {
                            if (this.IsPlayablePocket((int)(1326.0 * this.playingStyle.PreflopThreeBet)))
                            {
                                return this.RaiseOrAllIn(((this.context.MinRaise * 2) + this.context.MoneyToCall)
                                    + (this.stats.Callers * this.context.SmallBlind * 2));
                            }
                            else
                            {
                                return PlayerAction.CheckOrCall();
                            }
                        }
                        else if (this.stats.OpenRaiseOpportunity)
                        {
                            return this.RaiseOrAllIn(((this.context.MinRaise * 2) + this.context.MoneyToCall)
                                + (this.stats.Callers * this.context.SmallBlind * 2));
                        }
                        else
                        {
                            // TODO: 4bet, 5bet and so on
                            return PlayerAction.CheckOrCall();
                        }
                    }
                    else
                    {
                        return PlayerAction.CheckOrCall();
                    }
                }
                else if (this.IsPlayablePocket(relativePosition, numberOfOpponentsInHand, this.playingStyle.VPIP, 0.3))
                {
                    return PlayerAction.CheckOrCall();
                }
                else
                {
                    return PlayerAction.Fold();
                }
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
                    investment = investment < this.context.MinRaise ? this.context.MinRaise : investment;
                    var valueBet = RandomProvider.Next(
                        investment,
                        investment + (this.context.MinRaise * 5));
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

        public int RelativePosition()
        {
            var playersInHand = this.context.Opponents.Where(x => x.InHand)
                .Select(x => x.Position)
                .Union(new[] { this.context.Position })
                .OrderBy(k => k);
            return playersInHand.TakeWhile(x => x != this.context.Position).Count();
        }

        private int Investment(int pot, out bool nutHand, out bool tie)
        {
            var equity = this.calculator.Equity();
            var heroHandStrength = equity.First(x => x.Pocket.Mask == this.heroPocket.Mask);

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

        private bool IsPlayablePocket(int heroPosition, int numberOfOpponentsInHand, double range, double slope)
        {
            /*
             * The number of playable pockets depends on the position of the hero.
             * From the early position of playable pockets is less than from a late
             * position. In this case, the dependence of playable cards on the position
             * is quadratic.
            */
            var frequency = Distribution.FrequencyOfActionFromASpecificPosition(
                heroPosition, numberOfOpponentsInHand, range, slope);
            var numberOfPlayablePockets = (int)(1326.0 * (numberOfOpponentsInHand + 1) * frequency);

            return this.IsPlayablePocket(numberOfPlayablePockets);
        }

        private bool IsPlayablePocket(int numberOfPlayablePockets)
        {
            return new HoldemHand.PocketHands(
                PocketsFromStrongToWeak.Take(numberOfPlayablePockets).ToList()).Contains(this.heroPocket.Mask);
        }
    }
}

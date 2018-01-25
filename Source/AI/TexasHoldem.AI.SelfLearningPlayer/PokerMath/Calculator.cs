namespace TexasHoldem.AI.SelfLearningPlayer.PokerMath
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Cards;

    public class Calculator : ICalculator
    {
        private readonly ulong hero;

        private readonly IList<ulong> opponents;

        private readonly ulong communityCards;

        public Calculator(
            ICollection<Card> heroHoleCards, IList<ICollection<Card>> opponentsHoleCards, ICollection<Card> communityCards)
        {
            if (heroHoleCards == null)
            {
                throw new ArgumentNullException(nameof(heroHoleCards));
            }
            else if (heroHoleCards.Count != 2)
            {
                throw new ArgumentOutOfRangeException(nameof(heroHoleCards), "At least two hole cards are required");
            }

            if (opponentsHoleCards == null)
            {
                throw new ArgumentNullException(nameof(opponentsHoleCards));
            }
            else if (opponentsHoleCards.Any(x => x.Count != 2))
            {
                throw new ArgumentOutOfRangeException(nameof(opponentsHoleCards), "At least two hole cards are required");
            }

            if (communityCards == null)
            {
                throw new ArgumentNullException(nameof(communityCards));
            }

            this.hero = new CardAdapter(heroHoleCards).Mask;

            this.opponents = new List<ulong>();
            foreach (var item in opponentsHoleCards)
            {
                this.opponents.Add(new CardAdapter(item).Mask);
            }

            this.communityCards = new CardAdapter(communityCards).Mask;
        }

        public double Equity()
        {
            var potsWon = 0.0;
            var games = 0.0;
            var dead = this.hero | this.opponents.Aggregate((x, next) => next | x);
            foreach (var nextCommunityCards in HoldemHand.Hand.Hands(this.communityCards, dead, 5))
            {
                var heroBest = HoldemHand.Hand.Evaluate(this.hero | nextCommunityCards, 7);
                bool greaterThan = true;
                bool greaterThanEqual = true;
                var ties = 1.0;
                foreach (var oppHoleCards in this.opponents)
                {
                    var oppBest = HoldemHand.Hand.Evaluate(oppHoleCards | nextCommunityCards, 7);

                    if (heroBest < oppBest)
                    {
                        greaterThan = greaterThanEqual = false;
                        ties = 0;
                        break;
                    }
                    else if (heroBest <= oppBest)
                    {
                        greaterThan = false;
                        ties += 1.0;
                    }
                }

                if (greaterThan)
                {
                    potsWon += 1.0;
                }
                else if (greaterThanEqual)
                {
                    potsWon += 1.0 / ties;
                }

                games++;
            }

            return potsWon / games;
        }

        public double EV(int pot, int wager)
        {
            throw new NotImplementedException();
        }
    }
}
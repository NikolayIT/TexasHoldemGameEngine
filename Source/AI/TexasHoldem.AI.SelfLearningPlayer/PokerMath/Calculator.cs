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
            else if (opponentsHoleCards.Count == 0)
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
            double[] heroOdds;
            double[] opponentOdds;
            HoldemHand.Hand.HandWinOdds(
                new ulong[] { this.hero }, this.opponents.ToArray(), this.communityCards, out heroOdds, out opponentOdds);
            return heroOdds[0];
        }

        public double EV(int pot, int wager)
        {
            throw new NotImplementedException();
        }
    }
}
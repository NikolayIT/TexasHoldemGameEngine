namespace TexasHoldem.AI.SelfLearningPlayer.PokerMath
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Cards;

    public class Calculator : ICalculator
    {
        private readonly IList<IPocket> pockets;

        private readonly ulong communityCards;

        public Calculator(IList<IPocket> pockets, ICollection<Card> communityCards)
        {
            if (pockets == null)
            {
                throw new ArgumentNullException(nameof(pockets));
            }
            else if (pockets.Count < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(pockets), "At least two pockets are required");
            }

            if (communityCards == null)
            {
                throw new ArgumentNullException(nameof(communityCards));
            }

            this.pockets = pockets;
            this.communityCards = new CardAdapter(communityCards).Mask;
        }

        public ICollection<HandStrength> Equity()
        {
            var maskValue = new Dictionary<ulong, uint>();
            var potsWon = new Dictionary<ulong, double>();
            var dead = 0UL;

            foreach (var item in this.pockets)
            {
                maskValue.Add(item.Mask, 0);
                potsWon.Add(item.Mask, 0);
                dead |= item.Mask;
            }

            var games = 0.0;

            foreach (var nextCommunityCards in HoldemHand.Hand.Hands(this.communityCards, dead, 5))
            {
                foreach (var item in this.pockets)
                {
                    maskValue[item.Mask] = HoldemHand.Hand.Evaluate(item.Mask | nextCommunityCards, 7);
                }

                var winners = maskValue.GroupBy(x => x.Value)
                    .OrderByDescending(x => x.Key)
                    .First()
                    .Select(x => x.Key);

                foreach (var item in winners)
                {
                    potsWon[item] += 1.0 / winners.Count();
                }

                games++;
            }

            var result = new List<HandStrength>();
            foreach (var item in potsWon)
            {
                result.Add(new HandStrength(new Pocket(item.Key), item.Value / games));
            }

            return result;
        }
    }
}
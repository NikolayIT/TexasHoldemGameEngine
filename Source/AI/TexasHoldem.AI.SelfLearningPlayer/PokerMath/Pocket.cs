namespace TexasHoldem.AI.SelfLearningPlayer.PokerMath
{
    using System;
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;

    public class Pocket : IPocket
    {
        public Pocket(ICollection<Card> card)
        {
            if (card.Count != 2)
            {
                throw new ArgumentOutOfRangeException(nameof(card), "Two hole cards are required");
            }

            this.Mask = new CardAdapter(card).Mask;
        }

        public Pocket(ulong mask)
        {
            if (HoldemHand.Hand.BitCount(mask) != 2)
            {
                throw new ArgumentOutOfRangeException(nameof(mask), "Two hole cards are required");
            }

            this.Mask = mask;
        }

        public ulong Mask { get; }

        public string Text
        {
            get
            {
                return HoldemHand.Hand.MaskToString(this.Mask);
            }
        }
    }
}

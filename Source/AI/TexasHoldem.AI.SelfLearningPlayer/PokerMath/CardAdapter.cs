namespace TexasHoldem.AI.SelfLearningPlayer.PokerMath
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using TexasHoldem.Logic.Cards;

    public class CardAdapter
    {
        private readonly ICollection<Card> cards;

        public CardAdapter(ICollection<Card> cards)
        {
            if (cards == null)
            {
                throw new ArgumentNullException(nameof(cards));
            }

            this.cards = cards;
        }

        public ulong Mask
        {
            get
            {
                return HoldemHand.Hand.ParseHand(this.ToString());
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var item in this.cards)
            {
                sb.Append(CardExtensions.ToFriendlyString(item.Type).Replace("10", "T"));

                switch (item.Suit)
                {
                    case CardSuit.Club:
                        sb.Append('c');
                        break;
                    case CardSuit.Diamond:
                        sb.Append('d');
                        break;
                    case CardSuit.Heart:
                        sb.Append('h');
                        break;
                    case CardSuit.Spade:
                        sb.Append('s');
                        break;
                    default:
                        break;
                }
            }

            return sb.ToString();
        }
    }
}

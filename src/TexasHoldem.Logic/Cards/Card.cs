namespace TexasHoldem.Logic.Cards
{
    /// <summary>
    /// Immutable object to represent game card with suit and type.
    /// </summary>
    public class Card : IDeepCloneable<Card>
    {
        public Card(CardSuit suit, CardType type)
        {
            this.Suit = suit;
            this.Type = type;
        }

        public CardSuit Suit { get; }

        public CardType Type { get; }

        public static Card FromHashCode(int hashCode)
        {
            var suitId = hashCode / 13;
            return new Card((CardSuit)suitId, (CardType)(hashCode - (suitId * 13) + 2));
        }

        public override bool Equals(object obj)
        {
            var anotherCard = obj as Card;
            return anotherCard != null && this.Equals(anotherCard);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)this.Suit * 13) + (int)this.Type - 2;
            }
        }

        public Card DeepClone()
        {
            return new Card(this.Suit, this.Type);
        }

        public override string ToString()
        {
            return $"{this.Type.ToFriendlyString()}{this.Suit.ToFriendlyString()}";
        }

        private bool Equals(Card other)
        {
            return this.Suit == other.Suit && this.Type == other.Type;
        }
    }
}

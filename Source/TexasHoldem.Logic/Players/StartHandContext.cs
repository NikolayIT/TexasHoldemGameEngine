namespace TexasHoldem.Logic.Players
{
    using TexasHoldem.Logic.Cards;

    public class StartHandContext
    {
        public StartHandContext(Card firstCard, Card secondCard, int handNumber, int smallBlind)
        {
            this.FirstCard = firstCard;
            this.SecondCard = secondCard;
            this.HandNumber = handNumber;
            this.SmallBlind = smallBlind;
        }

        public Card FirstCard { get; set; }

        public Card SecondCard { get; set; }

        public int HandNumber { get; set; }

        public int SmallBlind { get; set; }
    }
}

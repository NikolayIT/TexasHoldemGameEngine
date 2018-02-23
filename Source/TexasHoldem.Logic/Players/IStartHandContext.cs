namespace TexasHoldem.Logic.Players
{
    using TexasHoldem.Logic.Cards;

    public interface IStartHandContext
    {
        Card FirstCard { get; }
        string FirstPlayerName { get; }
        int HandNumber { get; }
        int MoneyLeft { get; }
        Card SecondCard { get; }
        int SmallBlind { get; }
    }
}
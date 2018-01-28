namespace TexasHoldem.AI.SelfLearningPlayer.PokerMath
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;

    public interface IPocket
    {
        ICollection<Card> NativeType { get; }

        ulong Mask { get; }

        string Text { get; }
    }
}

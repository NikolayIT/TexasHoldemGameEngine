namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;

    public struct SidePot
    {
        public SidePot(int amount, IReadOnlyCollection<string> namesOfParticipants)
        {
            this.Amount = amount;
            this.NamesOfParticipants = namesOfParticipants;
        }

        public int Amount { get; }

        public IReadOnlyCollection<string> NamesOfParticipants { get; }
    }
}

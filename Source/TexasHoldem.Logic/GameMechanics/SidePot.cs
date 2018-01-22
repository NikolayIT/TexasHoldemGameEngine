namespace TexasHoldem.Logic.GameMechanics
{
    using System;
    using System.Collections.Generic;

    public struct SidePot
    {
        public SidePot(int amount, IReadOnlyCollection<string> namesOfParticipants)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "The size of the side pot should be greater than zero");
            }

            if (namesOfParticipants == null)
            {
                throw new ArgumentNullException(nameof(namesOfParticipants));
            }
            else if (namesOfParticipants.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(namesOfParticipants), "There must be at least one participant in the side pot");
            }

            this.Amount = amount;
            this.NamesOfParticipants = namesOfParticipants;
        }

        public int Amount { get; }

        public IReadOnlyCollection<string> NamesOfParticipants { get; }
    }
}

namespace TexasHoldem.Logic.GameMechanics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Players;

    public class MultiplePlayersTexasHoldemGame : BaseTexasHoldemGame
    {
        public MultiplePlayersTexasHoldemGame(IList<IPlayer> players, int initialMoney = 200)
            : base(players, initialMoney)
        {
            if (players.Count == 2)
            {
                throw new ArgumentException("The number of players must be between 3 and 10", nameof(players));
            }

            // Ensure the players have unique names
            var duplicateNames = players.GroupBy(x => x)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key.Name);

            if (duplicateNames.Count() > 0)
            {
                throw new ArgumentException($"Players have the same name: \"{string.Join(" ", duplicateNames.ToArray())}\"");
            }
        }

        public override void PlayHand()
        {
            var shifted = this.AllPlayers.ToList();

            // While at least two players have money
            while (this.AllPlayers.Count(x => x.PlayerMoney.Money > 0) > 1)
            {
                this.HandsPlayed++;

                // Cash game with unchanged size of the blinds
                var smallBlind = SmallBlinds[4];

                // Players are shifted in order of priority to make a move
                shifted.Add(shifted.First());
                shifted.RemoveAt(0);

                // Rotate players
                IHandLogic hand = new MultiplePlayersHandLogic(shifted, this.HandsPlayed, smallBlind);

                hand.Play();
            }
        }
    }
}
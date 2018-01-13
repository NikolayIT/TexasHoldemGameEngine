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
            var allPlayers = this.AllPlayers;

            // While at least two players have money
            while (allPlayers.Count(x => x.PlayerMoney.Money > 0) > 1)
            {
                this.HandsPlayed++;

                // Cash game with unchanged size of the blinds
                var smallBlind = SmallBlinds[0];

                // Players are sorted in order of priority to make a move
                var sorted = allPlayers.Select((s, i) => new KeyValuePair<IInternalPlayer, int>(s, i))
                    .OrderBy(k => (k.Value + this.HandsPlayed) % allPlayers.Count())
                    .Where(p => p.Key.PlayerMoney.Money > 0)
                    .Select(s => s.Key)
                    .ToList();

                // Rotate players
                IHandLogic hand = new MultiplePlayersHandLogic(sorted, this.HandsPlayed, smallBlind);

                hand.Play();
            }
        }
    }
}
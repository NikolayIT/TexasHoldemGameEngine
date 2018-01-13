namespace TexasHoldem.Logic.GameMechanics
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Players;

    public class HeadsUpTexasHoldemGame : BaseTexasHoldemGame
    {
        private readonly IInternalPlayer firstPlayer;

        private readonly IInternalPlayer secondPlayer;

        public HeadsUpTexasHoldemGame(IPlayer firstPlayer, IPlayer secondPlayer, int initialMoney = 1000)
            : base(new[] { firstPlayer, secondPlayer }, initialMoney)
        {
            if (firstPlayer == null)
            {
                throw new ArgumentNullException(nameof(firstPlayer));
            }

            if (secondPlayer == null)
            {
                throw new ArgumentNullException(nameof(secondPlayer));
            }

            // Ensure the players have unique names
            if (firstPlayer.Name == secondPlayer.Name)
            {
                throw new ArgumentException($"Both players have the same name: \"{firstPlayer.Name}\"");
            }

            this.firstPlayer = this.AllPlayers[0];
            this.secondPlayer = this.AllPlayers[1];
        }

        public override void PlayHand()
        {
            // While at least two players have money
            while (this.AllPlayers.Cast<InternalPlayer>().Count(x => x.PlayerMoney.Money > 0) > 1)
            {
                this.HandsPlayed++;

                // Every 10 hands the blind increases
                var smallBlind = SmallBlinds[(this.HandsPlayed - 1) / 10];

                // Rotate players
                IHandLogic hand = this.HandsPlayed % 2 == 1
                               ? new HeadsUpHandLogic(new[] { this.firstPlayer, this.secondPlayer }, this.HandsPlayed, smallBlind)
                               : new HeadsUpHandLogic(new[] { this.secondPlayer, this.firstPlayer }, this.HandsPlayed, smallBlind);

                hand.Play();
            }
        }
    }
}
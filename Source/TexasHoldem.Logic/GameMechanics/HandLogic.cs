namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Players;

    internal class HandLogic
    {
        private readonly IList<InternalPlayer> players;

        private readonly Deck deck;

        private int firstToPlay;

        public HandLogic(IList<InternalPlayer> players, int roundNumber)
        {
            this.players = players;

            // TODO: This logic is OK for 2 players but should be improved. What happens when one player drops?
            this.firstToPlay = (roundNumber - 1) % this.players.Count;

            this.deck = new Deck();
        }

        public void Play()
        {
            this.PreFlop();
            this.Flop();
            this.Turn();
            this.River();
        }

        // 1. pre-flop -> 2 cards for each player -> betting
        private void PreFlop()
        {
            foreach (var player in this.players)
            {
                var startHandContext = new StartHandContext
                                           {
                                               FirstCard = this.deck.GetNextCard(),
                                               SecondCard = this.deck.GetNextCard()
                                           };
                player.StartHand(startHandContext);
            }
        }

        // 2. flop -> 3 cards -> betting
        private void Flop()
        {
        }

        // 3. turn -> 1 card -> betting
        private void Turn()
        {
        }

        // 4. river -> 1 card -> betting
        private void River()
        {
        }
    }
}

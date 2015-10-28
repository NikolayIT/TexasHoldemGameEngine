namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Players;

    internal class HandLogic
    {
        private readonly IList<InternalPlayer> players;

        private readonly int smallBlind;

        private readonly Deck deck;

        private readonly ICollection<Card> communityCards;

        private readonly int firstToPlay;

        private int pot = 0;

        public HandLogic(IList<InternalPlayer> players, int roundNumber, int smallBlind)
        {
            this.players = players;
            this.smallBlind = smallBlind;

            // TODO: This logic is OK for 2 players but should be improved. What happens when one player drops?
            this.firstToPlay = (roundNumber - 1) % this.players.Count;

            this.deck = new Deck();
            this.communityCards = new List<Card>(5);
        }

        public void Play()
        {
            this.StartHandAndDealCards();
            this.PreFlop();
            this.Flop();
            this.Turn();
            this.River();
            this.DetermineWinner();
        }

        // 0. Start the hand and deal cards to each player
        private void StartHandAndDealCards()
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

        // 1. pre-flop -> blinds -> betting
        private void PreFlop()
        {
            var nextToPlay = this.firstToPlay;

            // Small blind
            // TODO: What if small blind is bigger than player's money?
            var smallBlindPlayer = this.players[nextToPlay % this.players.Count];
            smallBlindPlayer.Money -= this.smallBlind;
            this.pot += this.smallBlind;
            nextToPlay++;

            // Big blind
            // TODO: What if small blind is bigger than player's money?
            var bigBlindPlayer = this.players[nextToPlay % this.players.Count];
            bigBlindPlayer.Money -= this.smallBlind * 2;
            this.pot += this.smallBlind * 2;
            nextToPlay++;

            this.Betting(nextToPlay, this.smallBlind * 2);
        }

        // 2. flop -> 3 cards -> betting
        private void Flop()
        {
            for (var i = 0; i < 3; i++)
            {
                this.communityCards.Add(this.deck.GetNextCard());
            }

            var nextToPlay = this.firstToPlay;
            this.Betting(nextToPlay, 0);
        }

        // 3. turn -> 1 card -> betting
        private void Turn()
        {
            this.communityCards.Add(this.deck.GetNextCard());

            var nextToPlay = this.firstToPlay;
            this.Betting(nextToPlay, 0);
        }

        // 4. river -> 1 card -> betting
        private void River()
        {
            this.communityCards.Add(this.deck.GetNextCard());

            var nextToPlay = this.firstToPlay;
            this.Betting(nextToPlay, 0);
        }

        // 5. determine winner and give him/them the pot
        private void DetermineWinner()
        {
            // TODO: Implement
        }

        private void Betting(int firstToBet, int currentMaxBet)
        {
            // TODO: Implement
            while (true)
            {
                var player = this.players[firstToBet % this.players.Count];

                var getTurnContext = new GetTurnContext(this.communityCards);

                var turn = player.GetTurn(getTurnContext);

                firstToBet++;
            }
        }
    }
}

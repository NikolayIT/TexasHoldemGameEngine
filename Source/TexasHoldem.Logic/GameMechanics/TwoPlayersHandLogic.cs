namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Players;

    internal class TwoPlayersHandLogic
    {
        private readonly IList<InternalPlayer> allPlayers;

        private readonly int smallBlind;

        private readonly Deck deck;

        private readonly ICollection<Card> communityCards;

        private int pot = 0;

        public TwoPlayersHandLogic(InternalPlayer firstPlayer, InternalPlayer secondPlayer, int smallBlind)
        {
            this.allPlayers = new[] { firstPlayer, secondPlayer };
            this.smallBlind = smallBlind;
            this.deck = new Deck();
            this.communityCards = new List<Card>(5);
        }

        public void Play()
        {
            // 0. Start the hand and deal cards to each player
            foreach (var player in this.allPlayers)
            {
                var startHandContext = new StartHandContext
                {
                    FirstCard = this.deck.GetNextCard(),
                    SecondCard = this.deck.GetNextCard()
                };
                player.StartHand(startHandContext);
            }

            // 1. pre-flop -> blinds -> betting
            this.PreFlop();

            // 2. flop -> 3 cards -> betting
            this.PlayRound(GameRoundType.Flop, 3);

            // 3. turn -> 1 card -> betting
            this.PlayRound(GameRoundType.Turn, 1);

            // 4. river -> 1 card -> betting
            this.PlayRound(GameRoundType.River, 1);

            // 5. determine winner and give him/them the pot
            this.DetermineWinner();
            foreach (var player in this.allPlayers)
            {
                player.EndHand();
            }
        }

        private void PreFlop()
        {
            foreach (var player in this.allPlayers)
            {
                player.StartRound(new StartRoundContext(GameRoundType.PreFlop));
            }

            // Small blind
            // TODO: What if small blind is bigger than player's money?
            var smallBlindPlayer = this.allPlayers[0];
            smallBlindPlayer.Bet(this.smallBlind);
            this.pot += this.smallBlind;

            // Big blind
            // TODO: What if small blind is bigger than player's money?
            var bigBlindPlayer = this.allPlayers[1];
            bigBlindPlayer.Bet(this.smallBlind * 2);
            this.pot += this.smallBlind * 2;

            // Place pre-flop bets
            this.Betting();

            foreach (var player in this.allPlayers)
            {
                player.EndRound();
            }
        }

        private void PlayRound(GameRoundType gameRoundType, int communityCardsCount)
        {
            foreach (var player in this.allPlayers)
            {
                player.StartRound(new StartRoundContext(gameRoundType));
            }

            for (var i = 0; i < communityCardsCount; i++)
            {
                this.communityCards.Add(this.deck.GetNextCard());
            }

            this.Betting();

            foreach (var player in this.allPlayers)
            {
                player.EndRound();
            }
        }

        private void Betting()
        {
            // TODO: Implement
            var playerIndex = 0;
            while (true)
            {
                var player = this.allPlayers[playerIndex % this.allPlayers.Count];

                var getTurnContext = new GetTurnContext(this.communityCards);

                var turn = player.GetTurn(getTurnContext);

                playerIndex++;
            }
        }

        private void DetermineWinner()
        {
            // TODO: Implement
        }
    }
}

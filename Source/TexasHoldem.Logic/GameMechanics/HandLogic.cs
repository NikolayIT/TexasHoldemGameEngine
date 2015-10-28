namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Players;

    internal class HandLogic
    {
        private readonly int handNumber;

        private readonly int smallBlind;

        private readonly IList<InternalPlayer> players;

        private readonly Deck deck;

        private readonly ICollection<Card> communityCards;

        private readonly BettingLogic bettingLogic;

        public HandLogic(IList<InternalPlayer> players, int handNumber, int smallBlind)
        {
            this.handNumber = handNumber;
            this.smallBlind = smallBlind;
            this.players = players;
            this.deck = new Deck();
            this.communityCards = new List<Card>(5);
            this.bettingLogic = new BettingLogic(this.players, smallBlind);
        }

        public void Play()
        {
            // Start the hand and deal cards to each player
            foreach (var player in this.players)
            {
                var startHandContext = new StartHandContext(
                    this.deck.GetNextCard(),
                    this.deck.GetNextCard(),
                    this.handNumber,
                    this.smallBlind,
                    this.players[0].Name);
                player.StartHand(startHandContext);
            }

            // Pre-flop -> blinds -> betting
            this.PlayRound(GameRoundType.PreFlop, 0);

            // Flop -> 3 cards -> betting
            this.PlayRound(GameRoundType.Flop, 3);

            // Turn -> 1 card -> betting
            this.PlayRound(GameRoundType.Turn, 1);

            // River -> 1 card -> betting
            this.PlayRound(GameRoundType.River, 1);

            // TODO: Determine winner and give him/them the pot
            foreach (var player in this.players)
            {
                // TODO: Showdown
                player.EndHand();
            }
        }

        private void PlayRound(GameRoundType gameRoundType, int communityCardsCount)
        {
            foreach (var player in this.players)
            {
                player.StartRound(new StartRoundContext(gameRoundType));
            }

            for (var i = 0; i < communityCardsCount; i++)
            {
                this.communityCards.Add(this.deck.GetNextCard());
            }

            this.bettingLogic.Start(gameRoundType, this.communityCards);

            foreach (var player in this.players)
            {
                player.EndRound();
            }
        }
    }
}

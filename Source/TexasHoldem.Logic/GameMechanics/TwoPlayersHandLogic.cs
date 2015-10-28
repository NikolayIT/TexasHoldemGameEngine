namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;

    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Helpers;
    using TexasHoldem.Logic.Players;

    internal class TwoPlayersHandLogic
    {
        private static IActionValidator actionValidator = new ActionValidator();

        private readonly IList<InternalPlayer> allPlayers;

        private readonly Deck deck;

        private readonly ICollection<Card> communityCards;

        private readonly TwoPlayersBettingLogic bettingLogic;

        public TwoPlayersHandLogic(InternalPlayer firstPlayer, InternalPlayer secondPlayer, int smallBlind)
        {
            this.allPlayers = new[] { firstPlayer, secondPlayer };
            this.deck = new Deck();
            this.communityCards = new List<Card>(5);
            this.bettingLogic = new TwoPlayersBettingLogic(firstPlayer, secondPlayer, smallBlind);
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
            this.PlayRound(GameRoundType.PreFlop, 0);

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

            this.bettingLogic.Start(gameRoundType, this.communityCards);

            foreach (var player in this.allPlayers)
            {
                player.EndRound();
            }
        }

        private void DetermineWinner()
        {
            // TODO: Implement
        }
    }
}

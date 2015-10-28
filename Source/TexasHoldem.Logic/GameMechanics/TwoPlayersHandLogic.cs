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

            // Blinds
            this.Bet(this.allPlayers[0], this.smallBlind);
            this.Bet(this.allPlayers[1], this.smallBlind * 2);

            // Place pre-flop bets
            this.Betting();

            foreach (var player in this.allPlayers)
            {
                player.EndRound();
            }
        }

        private void Bet(InternalPlayer player, int amount)
        {
            // TODO: What if small blind is bigger than player's money?
            player.Bet(amount);
            this.pot += amount;
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

        // TODO: Implement
        private void Betting()
        {
            var potBeforeRound = this.pot;
            var bets = new List<PlayerActionAndName>();
            var playerIndex = 0;
            while (true)
            {
                var player = this.allPlayers[playerIndex % this.allPlayers.Count];

                var getTurnContext = new GetTurnContext(this.communityCards, potBeforeRound, bets.AsReadOnly());

                var action = player.GetTurn(getTurnContext);

                bets.Add(new PlayerActionAndName(player.Name, action));

                if (action.Type == PlayerActionType.Raise)
                {
                    this.Bet(player, action.Money);
                }
                else if (action.Type == PlayerActionType.Call)
                {
                    this.Bet(player, action.Money);
                }
                else if (action.Type == PlayerActionType.Check)
                {
                    // TODO: Is OK to check?
                }
                else
                {
                    // Fold
                    break;
                }

                playerIndex++;
            }
        }

        private void DetermineWinner()
        {
            // TODO: Implement
        }
    }
}

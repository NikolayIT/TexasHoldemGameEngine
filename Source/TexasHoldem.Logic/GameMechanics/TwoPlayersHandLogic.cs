namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Helpers;
    using TexasHoldem.Logic.Players;

    internal class TwoPlayersHandLogic
    {
        private readonly int handNumber;

        private readonly int smallBlind;

        private readonly IList<InternalPlayer> players;

        private readonly Deck deck;

        private readonly List<Card> communityCards;

        private readonly TwoPlayersBettingLogic bettingLogic;

        private Dictionary<string, ICollection<Card>> showdownCards;

        public TwoPlayersHandLogic(IList<InternalPlayer> players, int handNumber, int smallBlind)
        {
            this.handNumber = handNumber;
            this.smallBlind = smallBlind;
            this.players = players;
            this.deck = new Deck();
            this.communityCards = new List<Card>(5);
            this.bettingLogic = new TwoPlayersBettingLogic(this.players, smallBlind);
            this.showdownCards = new Dictionary<string, ICollection<Card>>();
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
                    player.PlayerMoney.Money,
                    this.smallBlind,
                    this.players[0].Name);
                player.StartHand(startHandContext);
            }

            // Pre-flop -> blinds -> betting
            this.PlayRound(GameRoundType.PreFlop, 0);

            // Flop -> 3 cards -> betting
            if (this.players.Count(x => x.PlayerMoney.InHand) > 1)
            {
                this.PlayRound(GameRoundType.Flop, 3);
            }

            // Turn -> 1 card -> betting
            if (this.players.Count(x => x.PlayerMoney.InHand) > 1)
            {
                this.PlayRound(GameRoundType.Turn, 1);
            }

            // River -> 1 card -> betting
            if (this.players.Count(x => x.PlayerMoney.InHand) > 1)
            {
                this.PlayRound(GameRoundType.River, 1);
            }

            this.DetermineWinnerAndAddPot(this.bettingLogic.Pot);

            foreach (var player in this.players)
            {
                player.EndHand(new EndHandContext(this.showdownCards));
            }
        }

        private void DetermineWinnerAndAddPot(int pot)
        {
            if (this.players.Count(x => x.PlayerMoney.InHand) == 1)
            {
                var winner = this.players.FirstOrDefault(x => x.PlayerMoney.InHand);
                winner.PlayerMoney.Money += pot;
            }
            else
            {
                // showdown
                foreach (var player in this.players)
                {
                    if (player.PlayerMoney.InHand)
                    {
                        this.showdownCards.Add(player.Name, player.Cards);
                    }
                }

                var betterHand = Helpers.CompareCards(
                    this.players[0].Cards.Concat(this.communityCards),
                    this.players[1].Cards.Concat(this.communityCards));
                if (betterHand > 0)
                {
                    this.players[0].PlayerMoney.Money += pot;
                }
                else if (betterHand < 0)
                {
                    this.players[1].PlayerMoney.Money += pot;
                }
                else
                {
                    this.players[0].PlayerMoney.Money += pot / 2;
                    this.players[1].PlayerMoney.Money += pot / 2;
                }
            }
        }

        private void PlayRound(GameRoundType gameRoundType, int communityCardsCount)
        {
            for (var i = 0; i < communityCardsCount; i++)
            {
                this.communityCards.Add(this.deck.GetNextCard());
            }

            foreach (var player in this.players)
            {
                var startRoundContext = new StartRoundContext(
                    gameRoundType,
                    this.communityCards.AsReadOnly(),
                    player.PlayerMoney.Money,
                    this.bettingLogic.Pot);
                player.StartRound(startRoundContext);
            }

            this.bettingLogic.Bet(gameRoundType);

            foreach (var player in this.players)
            {
                var endRoundContext = new EndRoundContext(this.bettingLogic.RoundBets);
                player.EndRound(endRoundContext);
            }
        }
    }
}

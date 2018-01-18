namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Helpers;
    using TexasHoldem.Logic.Players;

    internal abstract class BaseHandLogic : IHandLogic
    {
        private readonly int handNumber;

        private readonly int smallBlind;

        private readonly IList<IInternalPlayer> players;

        private readonly Deck deck;

        private readonly List<Card> communityCards;

        private readonly BaseBettingLogic bettingLogic;

        public BaseHandLogic(IList<IInternalPlayer> players, int handNumber, int smallBlind, BaseBettingLogic bettingLogic)
        {
            this.handNumber = handNumber;
            this.smallBlind = smallBlind;
            this.players = players;
            this.deck = new Deck();
            this.communityCards = new List<Card>(5);
            this.bettingLogic = bettingLogic;
            this.ShowdownCards = new Dictionary<string, ICollection<Card>>();
        }

        protected IReadOnlyList<IInternalPlayer> Players
        {
            get
            {
                return this.players.ToList();
            }
        }

        protected IReadOnlyList<Card> CommunityCards
        {
            get
            {
                return this.communityCards.ToList();
            }
        }

        protected Dictionary<string, ICollection<Card>> ShowdownCards { get; set; }

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

            this.DetermineWinnerAndAddPot(this.bettingLogic.Pot, this.bettingLogic.SidePots);

            foreach (var player in this.players)
            {
                player.EndHand(new EndHandContext(this.ShowdownCards));
            }
        }

        protected abstract void DetermineWinnerAndAddPot(int pot, IReadOnlyCollection<SidePot> sidePots);

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
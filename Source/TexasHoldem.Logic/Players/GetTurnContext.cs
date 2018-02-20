namespace TexasHoldem.Logic.Players
{
    using System.Collections.Generic;
    using TexasHoldem.Logic.GameMechanics;

    public class GetTurnContext : IGetTurnContext
    {
        public GetTurnContext(
            GameRoundType roundType,
            IReadOnlyCollection<PlayerActionAndName> previousRoundActions,
            int smallBlind,
            int moneyLeft,
            int currentPot,
            int myMoneyInTheRound,
            int currentMaxBet,
            int minRaise,
            Pot mainPot,
            List<Pot> sidePots)
        {
            this.RoundType = roundType;
            this.PreviousRoundActions = previousRoundActions;
            this.SmallBlind = smallBlind;
            this.MoneyLeft = moneyLeft;
            this.CurrentPot = currentPot;
            this.MyMoneyInTheRound = myMoneyInTheRound;
            this.CurrentMaxBet = currentMaxBet;
            this.MinRaise = minRaise;
            this.MainPot = mainPot;
            this.SidePots = sidePots;
        }

        public GameRoundType RoundType { get; }

        public IReadOnlyCollection<PlayerActionAndName> PreviousRoundActions { get; }

        public int SmallBlind { get; }

        public int MoneyLeft { get; }

        public int CurrentPot { get; }

        public int MyMoneyInTheRound { get; }

        public int CurrentMaxBet { get; }

        public bool CanCheck => this.MyMoneyInTheRound == this.CurrentMaxBet;

        public int MoneyToCall
        {
            get
            {
                var temp = this.CurrentMaxBet - this.MyMoneyInTheRound;
                if (temp >= this.MoneyLeft)
                {
                    // The player does not have enough money to make a full call
                    return this.MoneyLeft;
                }
                else
                {
                    return temp;
                }
            }
        }

        public bool IsAllIn => this.MoneyLeft <= 0;

        public int MinRaise { get; }

        public ICollection<PlayerActionType> AvailablePlayerOptions
        {
            get
            {
                if (this.MinRaise == -1 || this.MoneyToCall >= this.MoneyLeft)
                {
                    return new List<PlayerActionType> { PlayerActionType.Fold, PlayerActionType.CheckCall };
                }
                else
                {
                    return new List<PlayerActionType>
                    {
                        PlayerActionType.Fold,
                        PlayerActionType.CheckCall,
                        PlayerActionType.Raise
                    };
                }
            }
        }

        public Pot MainPot { get; }

        public List<Pot> SidePots { get; }
    }
}

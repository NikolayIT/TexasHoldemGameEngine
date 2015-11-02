namespace TexasHoldem.Logic.GameMechanics
{
    using System;

    using TexasHoldem.Logic.Players;

    public class InternalPlayerMoney
    {
        public InternalPlayerMoney(int startMoney)
        {
            this.Money = startMoney;
            this.NewHand();
            this.NewRound();
        }

        // Player money in the game
        public int Money { get; set; }

        // The amount of money the player is currently put in the pot
        public int CurrentlyInPot { get; private set; }

        // The amount of money the player is currently bet
        public int CurrentRoundBet { get; private set; }

        // False when player folds
        public bool InHand { get; private set; }

        // Player action is expected (some other player raised)
        public bool ShouldPlayInRound { get; set; }

        public void NewHand()
        {
            this.CurrentlyInPot = 0;
            this.CurrentRoundBet = 0;
            this.InHand = true;
            this.ShouldPlayInRound = true;
        }

        public void NewRound()
        {
            this.CurrentRoundBet = 0;
            if (this.InHand)
            {
                this.ShouldPlayInRound = true;
            }
        }

        // TODO: Currently there is no limit in the raise amount as long as it is positive number
        public PlayerAction DoPlayerAction(PlayerAction action, int maxMoneyPerPlayer)
        {
            if (action.Type == PlayerActionType.Raise)
            {
                this.CallTo(maxMoneyPerPlayer);

                if (this.Money <= 0)
                {
                    return PlayerAction.CheckOrCall();
                }

                if (this.Money > action.Money)
                {
                    this.PlaceMoney(action.Money);
                }
                else
                {
                    // All-in
                    action.Money = this.Money;
                    this.PlaceMoney(action.Money);
                }
            }
            else if (action.Type == PlayerActionType.CheckCall)
            {
                this.CallTo(maxMoneyPerPlayer);
            }
            else //// PlayerActionType.Fold
            {
                this.InHand = false;
                this.ShouldPlayInRound = false;
            }

            return action;
        }

        public void NormalizeBets(int moneyPerPlayer)
        {
            if (moneyPerPlayer < this.CurrentRoundBet)
            {
                var diff = this.CurrentRoundBet - moneyPerPlayer;
                this.PlaceMoney(-diff);
            }
        }

        private void PlaceMoney(int money)
        {
            this.CurrentRoundBet += money;
            this.CurrentlyInPot += money;
            this.Money -= money;
        }

        private void CallTo(int maxMoneyPerPlayer)
        {
            var moneyToPay = Math.Min(this.CurrentRoundBet + this.Money, maxMoneyPerPlayer);
            var diff = moneyToPay - this.CurrentRoundBet;
            this.PlaceMoney(diff);
        }
    }
}

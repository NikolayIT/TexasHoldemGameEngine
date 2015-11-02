namespace TexasHoldem.Logic.GameMechanics
{
    using TexasHoldem.Logic.Players;

    public class InternalPlayerMoney
    {
        public InternalPlayerMoney(int startMoney)
        {
            this.Money = startMoney;
            this.NewHand();
            this.NewRound();
        }

        public int Money { get; set; }

        public int CurrentlyInPot { get; private set; }

        public int CurrentRoundBet { get; private set; }

        public bool InHand { get; set; }

        public bool ShouldPlayInRound { get; set; }

        public void NewHand()
        {
            this.CurrentlyInPot = 0;
            this.CurrentRoundBet = 0;
            this.InHand = true;
        }

        public void NewRound()
        {
            this.CurrentRoundBet = 0;
            if (this.InHand)
            {
                this.ShouldPlayInRound = true;
            }
        }

        public PlayerAction DoPlayerAction(PlayerAction action, int maxMoneyPerPlayer)
        {
            if (action.Type == PlayerActionType.Raise)
            {
                // TODO: What if maxMoneyPerPlayer is bigger than player's money?
                this.Call(maxMoneyPerPlayer);

                if (this.Money <= 0)
                {
                    return PlayerAction.CheckOrCall();
                }

                // TODO: Min raise?
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
                // TODO: What if maxMoneyPerPlayer is bigger than player's money?
                this.Call(maxMoneyPerPlayer);
            }
            else //// PlayerActionType.Fold
            {
                this.InHand = false;
            }

            return action;
        }

        private void PlaceMoney(int money)
        {
            this.CurrentRoundBet += money;
            this.CurrentlyInPot += money;
            this.Money -= money;
        }

        private void Call(int maxMoneyPerPlayer)
        {
            var diff = maxMoneyPerPlayer - this.CurrentRoundBet;
            this.PlaceMoney(diff);
        }
    }
}

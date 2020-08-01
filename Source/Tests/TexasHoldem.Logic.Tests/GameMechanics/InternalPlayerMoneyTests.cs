namespace TexasHoldem.Logic.Tests.GameMechanics
{
    using TexasHoldem.Logic.GameMechanics;
    using TexasHoldem.Logic.Players;

    using Xunit;

    public class InternalPlayerMoneyTests
    {
        [Fact]
        public void ConstructorShouldSetCorrectValuesToProperties()
        {
            const int StartMoney = 3777;
            var internalPlayerMoney = new InternalPlayerMoney(StartMoney);
            Assert.Equal(StartMoney, internalPlayerMoney.Money);
            Assert.Equal(0, internalPlayerMoney.CurrentlyInPot);
            Assert.Equal(0, internalPlayerMoney.CurrentRoundBet);
            Assert.True(internalPlayerMoney.InHand);
            Assert.True(internalPlayerMoney.ShouldPlayInRound);
        }

        [Fact]
        public void NewHandShouldSetCorrectValuesToProperties()
        {
            var internalPlayerMoney = new InternalPlayerMoney(3777);
            internalPlayerMoney.DoPlayerAction(PlayerAction.Raise(10), 0);
            internalPlayerMoney.DoPlayerAction(PlayerAction.Fold(), 20);

            internalPlayerMoney.NewHand();

            Assert.Equal(0, internalPlayerMoney.CurrentlyInPot);
            Assert.Equal(0, internalPlayerMoney.CurrentRoundBet);
            Assert.True(internalPlayerMoney.InHand);
            Assert.True(internalPlayerMoney.ShouldPlayInRound);
        }

        [Fact]
        public void NewRoundShouldSetCorrectValuesToProperties()
        {
            var internalPlayerMoney = new InternalPlayerMoney(3777);
            internalPlayerMoney.DoPlayerAction(PlayerAction.Raise(10), 0);
            internalPlayerMoney.DoPlayerAction(PlayerAction.Fold(), 20);

            internalPlayerMoney.NewRound();

            Assert.Equal(10, internalPlayerMoney.CurrentlyInPot);
            Assert.Equal(0, internalPlayerMoney.CurrentRoundBet);
            Assert.False(internalPlayerMoney.InHand, "InHand should be false");
            Assert.False(internalPlayerMoney.ShouldPlayInRound, "ShouldPlayInRound should be false");
        }

        [Fact]
        public void NewRoundShouldResetPlayInRoundPropertyIfPlayerIsStillInGame()
        {
            var internalPlayerMoney = new InternalPlayerMoney(3777);
            internalPlayerMoney.DoPlayerAction(PlayerAction.Raise(10), 0);
            internalPlayerMoney.ShouldPlayInRound = false;

            internalPlayerMoney.NewRound();

            Assert.True(internalPlayerMoney.InHand, "InHand should be false");
            Assert.True(internalPlayerMoney.ShouldPlayInRound, "ShouldPlayInRound should be false");
        }

        [Fact]
        public void RaiseShouldPayCurrentBets()
        {
            var internalPlayerMoney = new InternalPlayerMoney(1000);

            var action = internalPlayerMoney.DoPlayerAction(PlayerAction.Raise(100), 120);
            Assert.Equal(PlayerActionType.Raise, action.Type);

            Assert.Equal(1000 - 100 - 120, internalPlayerMoney.Money);
            Assert.Equal(220, internalPlayerMoney.CurrentRoundBet);
            Assert.Equal(220, internalPlayerMoney.CurrentlyInPot);
        }

        [Fact]
        public void OneCallShouldPayCurrentBets()
        {
            var internalPlayerMoney = new InternalPlayerMoney(1000);

            var action = internalPlayerMoney.DoPlayerAction(PlayerAction.CheckOrCall(), 80);
            Assert.Equal(PlayerActionType.CheckCall, action.Type);

            Assert.Equal(1000 - 80, internalPlayerMoney.Money);
            Assert.Equal(80, internalPlayerMoney.CurrentRoundBet);
            Assert.Equal(80, internalPlayerMoney.CurrentlyInPot);
        }

        [Fact]
        public void TwoCallsShouldPayCurrentBets()
        {
            var internalPlayerMoney = new InternalPlayerMoney(1000);

            var action1 = internalPlayerMoney.DoPlayerAction(PlayerAction.CheckOrCall(), 80);
            Assert.Equal(PlayerActionType.CheckCall, action1.Type);

            var action2 = internalPlayerMoney.DoPlayerAction(PlayerAction.CheckOrCall(), 200);
            Assert.Equal(PlayerActionType.CheckCall, action2.Type);

            Assert.Equal(1000 - 200, internalPlayerMoney.Money);
            Assert.Equal(200, internalPlayerMoney.CurrentRoundBet);
            Assert.Equal(200, internalPlayerMoney.CurrentlyInPot);
        }

        [Fact]
        public void FoldShouldNotPayCurrentBets()
        {
            var internalPlayerMoney = new InternalPlayerMoney(1000);

            var action = internalPlayerMoney.DoPlayerAction(PlayerAction.Fold(), 100);

            Assert.Equal(PlayerActionType.Fold, action.Type);

            Assert.Equal(1000, internalPlayerMoney.Money);
            Assert.Equal(0, internalPlayerMoney.CurrentRoundBet);
            Assert.Equal(0, internalPlayerMoney.CurrentlyInPot);
        }

        [Fact]
        public void CallWhenNoMoney()
        {
            var internalPlayerMoney = new InternalPlayerMoney(0);

            var action = internalPlayerMoney.DoPlayerAction(PlayerAction.CheckOrCall(), 120);

            Assert.Equal(PlayerActionType.CheckCall, action.Type);

            Assert.Equal(0, internalPlayerMoney.Money);
            Assert.Equal(0, internalPlayerMoney.CurrentRoundBet);
            Assert.Equal(0, internalPlayerMoney.CurrentlyInPot);
        }

        [Fact]
        public void CallWhenNotSufficientMoney()
        {
            var internalPlayerMoney = new InternalPlayerMoney(100);

            internalPlayerMoney.DoPlayerAction(PlayerAction.CheckOrCall(), 80);
            var action = internalPlayerMoney.DoPlayerAction(PlayerAction.CheckOrCall(), 120);

            Assert.Equal(PlayerActionType.CheckCall, action.Type);

            Assert.Equal(0, internalPlayerMoney.Money);
            Assert.Equal(100, internalPlayerMoney.CurrentRoundBet);
            Assert.Equal(100, internalPlayerMoney.CurrentlyInPot);
        }

        [Fact]
        public void CallWhenNotSufficientMoneyAndDiffIs0()
        {
            var internalPlayerMoney = new InternalPlayerMoney(100);

            internalPlayerMoney.DoPlayerAction(PlayerAction.CheckOrCall(), 100);
            var action = internalPlayerMoney.DoPlayerAction(PlayerAction.CheckOrCall(), 120);

            Assert.Equal(PlayerActionType.CheckCall, action.Type);

            Assert.Equal(0, internalPlayerMoney.Money);
            Assert.Equal(100, internalPlayerMoney.CurrentRoundBet);
            Assert.Equal(100, internalPlayerMoney.CurrentlyInPot);
        }

        [Fact]
        public void RaiseWhenNotSufficientMoney()
        {
            var internalPlayerMoney = new InternalPlayerMoney(10);

            var action = internalPlayerMoney.DoPlayerAction(PlayerAction.Raise(20), 0);

            Assert.Equal(PlayerActionType.Raise, action.Type);
            Assert.Equal(10, action.Money);

            Assert.Equal(0, internalPlayerMoney.Money);
            Assert.Equal(10, internalPlayerMoney.CurrentRoundBet);
            Assert.Equal(10, internalPlayerMoney.CurrentlyInPot);
        }

        [Fact]
        public void RaiseWhenNotSufficientMoneyAfterACall()
        {
            var internalPlayerMoney = new InternalPlayerMoney(10);

            internalPlayerMoney.DoPlayerAction(PlayerAction.CheckOrCall(), 5);
            var action = internalPlayerMoney.DoPlayerAction(PlayerAction.Raise(20), 5);

            Assert.Equal(PlayerActionType.Raise, action.Type);
            Assert.Equal(5, action.Money);

            Assert.Equal(0, internalPlayerMoney.Money);
            Assert.Equal(10, internalPlayerMoney.CurrentRoundBet);
            Assert.Equal(10, internalPlayerMoney.CurrentlyInPot);
        }

        [Fact]
        public void RaiseWhenNoMoney()
        {
            var internalPlayerMoney = new InternalPlayerMoney(0);

            var action = internalPlayerMoney.DoPlayerAction(PlayerAction.Raise(20), 0);

            Assert.Equal(PlayerActionType.CheckCall, action.Type);

            Assert.Equal(0, action.Money);
            Assert.Equal(0, internalPlayerMoney.Money);
            Assert.Equal(0, internalPlayerMoney.CurrentRoundBet);
            Assert.Equal(0, internalPlayerMoney.CurrentlyInPot);
        }

        [Fact]
        public void RaiseWhenNoMoneyAndPreviousBet()
        {
            var internalPlayerMoney = new InternalPlayerMoney(0);

            var action = internalPlayerMoney.DoPlayerAction(PlayerAction.Raise(20), 10);

            Assert.Equal(PlayerActionType.CheckCall, action.Type);

            Assert.Equal(0, internalPlayerMoney.Money);
            Assert.Equal(0, internalPlayerMoney.CurrentRoundBet);
            Assert.Equal(0, internalPlayerMoney.CurrentlyInPot);
        }

        [Fact]
        public void NormalizeBetsShouldReturnMoneyToThePlayerWhenOtherPlayersDoNotHaveSufficientFunds()
        {
            var internalPlayerMoney = new InternalPlayerMoney(1000);
            internalPlayerMoney.DoPlayerAction(PlayerAction.CheckOrCall(), 200);

            internalPlayerMoney.NormalizeBets(100);

            Assert.Equal(900, internalPlayerMoney.Money);
            Assert.Equal(100, internalPlayerMoney.CurrentRoundBet);
            Assert.Equal(100, internalPlayerMoney.CurrentlyInPot);
        }

        [Fact]
        public void NormalizeBetsShouldNotChangeDataWhenSameValueAsCurrentRoundBet()
        {
            var internalPlayerMoney = new InternalPlayerMoney(1000);
            internalPlayerMoney.DoPlayerAction(PlayerAction.CheckOrCall(), 200);

            internalPlayerMoney.NormalizeBets(200);

            Assert.Equal(800, internalPlayerMoney.Money);
            Assert.Equal(200, internalPlayerMoney.CurrentRoundBet);
            Assert.Equal(200, internalPlayerMoney.CurrentlyInPot);
        }
    }
}

namespace TexasHoldem.Logic.Tests.GameMechanics
{
    using NUnit.Framework;

    using TexasHoldem.Logic.GameMechanics;
    using TexasHoldem.Logic.Players;

    [TestFixture]
    public class InternalPlayerMoneyTests
    {
        [Test]
        public void ConstructorShouldSetCorrectValuesToProperties()
        {
            const int StartMoney = 3777;
            var internalPlayerMoney = new InternalPlayerMoney(StartMoney);
            Assert.AreEqual(StartMoney, internalPlayerMoney.Money);
            Assert.AreEqual(0, internalPlayerMoney.CurrentlyInPot);
            Assert.AreEqual(0, internalPlayerMoney.CurrentRoundBet);
            Assert.AreEqual(true, internalPlayerMoney.InHand);
            Assert.AreEqual(true, internalPlayerMoney.ShouldPlayInRound);
        }

        [Test]
        public void NewHandShouldSetCorrectValuesToProperties()
        {
            var internalPlayerMoney = new InternalPlayerMoney(3777);
            internalPlayerMoney.DoPlayerAction(PlayerAction.Raise(10), 0);
            internalPlayerMoney.DoPlayerAction(PlayerAction.Fold(), 20);

            internalPlayerMoney.NewHand();

            Assert.AreEqual(0, internalPlayerMoney.CurrentlyInPot);
            Assert.AreEqual(0, internalPlayerMoney.CurrentRoundBet);
            Assert.AreEqual(true, internalPlayerMoney.InHand);
            Assert.AreEqual(true, internalPlayerMoney.ShouldPlayInRound);
        }

        [Test]
        public void NewRoundShouldSetCorrectValuesToProperties()
        {
            var internalPlayerMoney = new InternalPlayerMoney(3777);
            internalPlayerMoney.DoPlayerAction(PlayerAction.Raise(10), 0);
            internalPlayerMoney.DoPlayerAction(PlayerAction.Fold(), 20);

            internalPlayerMoney.NewRound();

            Assert.AreEqual(10, internalPlayerMoney.CurrentlyInPot);
            Assert.AreEqual(0, internalPlayerMoney.CurrentRoundBet);
            Assert.AreEqual(false, internalPlayerMoney.InHand, "InHand should be false");
            Assert.AreEqual(false, internalPlayerMoney.ShouldPlayInRound, "ShouldPlayInRound should be false");
        }

        [Test]
        public void NewRoundShouldResetPlayInRoundPropertyIfPlayerIsStillInGame()
        {
            var internalPlayerMoney = new InternalPlayerMoney(3777);
            internalPlayerMoney.DoPlayerAction(PlayerAction.Raise(10), 0);
            internalPlayerMoney.ShouldPlayInRound = false;

            internalPlayerMoney.NewRound();

            Assert.AreEqual(true, internalPlayerMoney.InHand, "InHand should be false");
            Assert.AreEqual(true, internalPlayerMoney.ShouldPlayInRound, "ShouldPlayInRound should be false");
        }

        [Test]
        public void RaiseShouldPayCurrentBets()
        {
            var internalPlayerMoney = new InternalPlayerMoney(1000);

            var action = internalPlayerMoney.DoPlayerAction(PlayerAction.Raise(100), 120);
            Assert.AreEqual(PlayerActionType.Raise, action.Type);

            Assert.AreEqual(1000 - 100 - 120, internalPlayerMoney.Money);
            Assert.AreEqual(220, internalPlayerMoney.CurrentRoundBet);
            Assert.AreEqual(220, internalPlayerMoney.CurrentlyInPot);
        }

        [Test]
        public void OneCallShouldPayCurrentBets()
        {
            var internalPlayerMoney = new InternalPlayerMoney(1000);

            var action = internalPlayerMoney.DoPlayerAction(PlayerAction.CheckOrCall(), 80);
            Assert.AreEqual(PlayerActionType.CheckCall, action.Type);

            Assert.AreEqual(1000 - 80, internalPlayerMoney.Money);
            Assert.AreEqual(80, internalPlayerMoney.CurrentRoundBet);
            Assert.AreEqual(80, internalPlayerMoney.CurrentlyInPot);
        }

        [Test]
        public void TwoCallsShouldPayCurrentBets()
        {
            var internalPlayerMoney = new InternalPlayerMoney(1000);

            var action1 = internalPlayerMoney.DoPlayerAction(PlayerAction.CheckOrCall(), 80);
            Assert.AreEqual(PlayerActionType.CheckCall, action1.Type);

            var action2 = internalPlayerMoney.DoPlayerAction(PlayerAction.CheckOrCall(), 200);
            Assert.AreEqual(PlayerActionType.CheckCall, action2.Type);

            Assert.AreEqual(1000 - 200, internalPlayerMoney.Money);
            Assert.AreEqual(200, internalPlayerMoney.CurrentRoundBet);
            Assert.AreEqual(200, internalPlayerMoney.CurrentlyInPot);
        }

        [Test]
        public void FoldShouldNotPayCurrentBets()
        {
            var internalPlayerMoney = new InternalPlayerMoney(1000);

            var action = internalPlayerMoney.DoPlayerAction(PlayerAction.Fold(), 100);

            Assert.AreEqual(PlayerActionType.Fold, action.Type);

            Assert.AreEqual(1000, internalPlayerMoney.Money);
            Assert.AreEqual(0, internalPlayerMoney.CurrentRoundBet);
            Assert.AreEqual(0, internalPlayerMoney.CurrentlyInPot);
        }

        [Test]
        public void CallWhenNoMoney()
        {
            var internalPlayerMoney = new InternalPlayerMoney(0);

            var action = internalPlayerMoney.DoPlayerAction(PlayerAction.CheckOrCall(), 120);

            Assert.AreEqual(PlayerActionType.CheckCall, action.Type);

            Assert.AreEqual(0, internalPlayerMoney.Money);
            Assert.AreEqual(0, internalPlayerMoney.CurrentRoundBet);
            Assert.AreEqual(0, internalPlayerMoney.CurrentlyInPot);
        }

        [Test]
        public void CallWhenNotSufficientMoney()
        {
            var internalPlayerMoney = new InternalPlayerMoney(100);

            internalPlayerMoney.DoPlayerAction(PlayerAction.CheckOrCall(), 80);
            var action = internalPlayerMoney.DoPlayerAction(PlayerAction.CheckOrCall(), 120);

            Assert.AreEqual(PlayerActionType.CheckCall, action.Type);

            Assert.AreEqual(0, internalPlayerMoney.Money);
            Assert.AreEqual(100, internalPlayerMoney.CurrentRoundBet);
            Assert.AreEqual(100, internalPlayerMoney.CurrentlyInPot);
        }

        [Test]
        public void CallWhenNotSufficientMoneyAndDiffIs0()
        {
            var internalPlayerMoney = new InternalPlayerMoney(100);

            internalPlayerMoney.DoPlayerAction(PlayerAction.CheckOrCall(), 100);
            var action = internalPlayerMoney.DoPlayerAction(PlayerAction.CheckOrCall(), 120);

            Assert.AreEqual(PlayerActionType.CheckCall, action.Type);

            Assert.AreEqual(0, internalPlayerMoney.Money);
            Assert.AreEqual(100, internalPlayerMoney.CurrentRoundBet);
            Assert.AreEqual(100, internalPlayerMoney.CurrentlyInPot);
        }

        [Test]
        public void RaiseWhenNotSufficientMoney()
        {
            var internalPlayerMoney = new InternalPlayerMoney(10);

            var action = internalPlayerMoney.DoPlayerAction(PlayerAction.Raise(20), 0);

            Assert.AreEqual(PlayerActionType.Raise, action.Type);
            Assert.AreEqual(10, action.Money);

            Assert.AreEqual(0, internalPlayerMoney.Money);
            Assert.AreEqual(10, internalPlayerMoney.CurrentRoundBet);
            Assert.AreEqual(10, internalPlayerMoney.CurrentlyInPot);
        }

        [Test]
        public void RaiseWhenNotSufficientMoneyAfterACall()
        {
            var internalPlayerMoney = new InternalPlayerMoney(10);

            internalPlayerMoney.DoPlayerAction(PlayerAction.CheckOrCall(), 5);
            var action = internalPlayerMoney.DoPlayerAction(PlayerAction.Raise(20), 5);

            Assert.AreEqual(PlayerActionType.Raise, action.Type);
            Assert.AreEqual(5, action.Money);

            Assert.AreEqual(0, internalPlayerMoney.Money);
            Assert.AreEqual(10, internalPlayerMoney.CurrentRoundBet);
            Assert.AreEqual(10, internalPlayerMoney.CurrentlyInPot);
        }

        [Test]
        public void RaiseWhenNoMoney()
        {
            var internalPlayerMoney = new InternalPlayerMoney(0);

            var action = internalPlayerMoney.DoPlayerAction(PlayerAction.Raise(20), 0);

            Assert.AreEqual(PlayerActionType.CheckCall, action.Type);

            Assert.AreEqual(0, action.Money);
            Assert.AreEqual(0, internalPlayerMoney.Money);
            Assert.AreEqual(0, internalPlayerMoney.CurrentRoundBet);
            Assert.AreEqual(0, internalPlayerMoney.CurrentlyInPot);
        }

        [Test]
        public void RaiseWhenNoMoneyAndPreviousBet()
        {
            var internalPlayerMoney = new InternalPlayerMoney(0);

            var action = internalPlayerMoney.DoPlayerAction(PlayerAction.Raise(20), 10);

            Assert.AreEqual(PlayerActionType.CheckCall, action.Type);

            Assert.AreEqual(0, internalPlayerMoney.Money);
            Assert.AreEqual(0, internalPlayerMoney.CurrentRoundBet);
            Assert.AreEqual(0, internalPlayerMoney.CurrentlyInPot);
        }

        [Test]
        public void NormalizeBetsShouldReturnMoneyToThePlayerWhenOtherPlayersDoNotHaveSufficientFunds()
        {
            var internalPlayerMoney = new InternalPlayerMoney(1000);
            internalPlayerMoney.DoPlayerAction(PlayerAction.CheckOrCall(), 200);

            internalPlayerMoney.NormalizeBets(100);

            Assert.AreEqual(900, internalPlayerMoney.Money);
            Assert.AreEqual(100, internalPlayerMoney.CurrentRoundBet);
            Assert.AreEqual(100, internalPlayerMoney.CurrentlyInPot);
        }

        [Test]
        public void NormalizeBetsShouldNotChangeDataWhenSameValueAsCurrentRoundBet()
        {
            var internalPlayerMoney = new InternalPlayerMoney(1000);
            internalPlayerMoney.DoPlayerAction(PlayerAction.CheckOrCall(), 200);

            internalPlayerMoney.NormalizeBets(200);

            Assert.AreEqual(800, internalPlayerMoney.Money);
            Assert.AreEqual(200, internalPlayerMoney.CurrentRoundBet);
            Assert.AreEqual(200, internalPlayerMoney.CurrentlyInPot);
        }
    }
}

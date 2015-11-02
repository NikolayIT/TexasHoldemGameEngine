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

        // TODO: Check values when not sufficient money
    }
}

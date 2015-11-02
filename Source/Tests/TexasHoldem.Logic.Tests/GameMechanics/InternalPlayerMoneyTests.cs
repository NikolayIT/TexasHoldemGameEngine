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

    }
}

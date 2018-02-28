namespace TexasHoldem.Logic.Tests.GameMechanics
{
    using NUnit.Framework;
    using TexasHoldem.Logic.GameMechanics;

    [TestFixture]
    public class MinRaiseTests
    {
        [Test]
        public void MinRaiseShouldReturnCorrectValue()
        {
            var minRaise = new MinRaise(1);

            minRaise.Update("utg", 2, 2); // limp
            Assert.AreEqual(string.Empty, minRaise.AggressorName);
            Assert.AreEqual(2, minRaise.Amount("mp"));

            minRaise.Update("mp", 2, 6); // raise to 6
            Assert.AreEqual("mp", minRaise.AggressorName);
            Assert.AreEqual(4, minRaise.Amount("co"));

            minRaise.Update("co", 6, 8); // raise allin to 8 (player does not raise enough)
            Assert.AreEqual("mp", minRaise.AggressorName);
            Assert.AreEqual(4, minRaise.Amount("btn"));

            minRaise.Update("btn", 8, 8); // call
            Assert.AreEqual("mp", minRaise.AggressorName);
            Assert.AreEqual(0, minRaise.Amount("mp"));

            // next round
            minRaise.Reset();

            minRaise.Update("utg", 0, 1); // bet allin (player does not bet enough)
            Assert.AreEqual(string.Empty, minRaise.AggressorName);
            Assert.AreEqual(2, minRaise.Amount("mp"));

            minRaise.Update("mp", 1, 3); // raise to 3
            Assert.AreEqual("mp", minRaise.AggressorName);
            Assert.AreEqual(2, minRaise.Amount("co"));
        }
    }
}

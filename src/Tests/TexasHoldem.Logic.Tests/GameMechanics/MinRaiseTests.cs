namespace TexasHoldem.Logic.Tests.GameMechanics
{
    using TexasHoldem.Logic.GameMechanics;

    using Xunit;

    public class MinRaiseTests
    {
        [Fact]
        public void MinRaiseShouldReturnCorrectValue()
        {
            var minRaise = new MinRaise(1);

            minRaise.Update("utg", 2, 2); // limp
            Assert.Equal(string.Empty, minRaise.AggressorName);
            Assert.Equal(2, minRaise.Amount("mp"));

            minRaise.Update("mp", 2, 6); // raise to 6
            Assert.Equal("mp", minRaise.AggressorName);
            Assert.Equal(4, minRaise.Amount("co"));

            minRaise.Update("co", 6, 8); // raise allin to 8 (player does not raise enough)
            Assert.Equal("mp", minRaise.AggressorName);
            Assert.Equal(4, minRaise.Amount("btn"));

            minRaise.Update("btn", 8, 8); // call
            Assert.Equal("mp", minRaise.AggressorName);
            Assert.Equal(0, minRaise.Amount("mp"));

            // next round
            minRaise.Reset();

            minRaise.Update("utg", 0, 1); // bet allin (player does not bet enough)
            Assert.Equal(string.Empty, minRaise.AggressorName);
            Assert.Equal(2, minRaise.Amount("mp"));

            minRaise.Update("mp", 1, 3); // raise to 3
            Assert.Equal("mp", minRaise.AggressorName);
            Assert.Equal(2, minRaise.Amount("co"));
        }
    }
}

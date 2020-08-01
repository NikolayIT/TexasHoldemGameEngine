namespace TexasHoldem.Logic.Tests.GameMechanics
{
    using System;

    using Moq;

    using TexasHoldem.Logic.GameMechanics;
    using TexasHoldem.Logic.Players;

    using Xunit;

    public class TexasHoldemGameTests
    {
        [Fact]
        public void ConstructorShouldThrowArgumentNullExceptionWhenIncorrectFirstPlayer()
        {
            IPlayer firstPlayer = null;
            var mockedSecondPlayer = new Mock<IPlayer>();
            var initialMoney = 1000;
            Assert.Throws<ArgumentNullException>(
                () => { var twoPlayersGame = new TexasHoldemGame(firstPlayer, mockedSecondPlayer.Object, initialMoney); });
        }

        [Fact]
        public void ConstructorShouldThrowArgumentNullExceptionWhenIncorrectSecondPlayer()
        {
            IPlayer secondPlayer = null;
            var mockedFirstPlayer = new Mock<IPlayer>();
            var initialMoney = 1000;
            Assert.Throws<ArgumentNullException>(
                () => { var twoPlayersGame = new TexasHoldemGame(mockedFirstPlayer.Object, secondPlayer, initialMoney); });
        }

        [Fact]
        public void ConstructorShouldThrowArgumentOutOfRangeExceptionWhenInitialMoneyAreNegative()
        {
            var mockedSecondPlayer = new Mock<IPlayer>();
            var mockedFirstPlayer = new Mock<IPlayer>();
            var initialMoney = -100;
            Assert.Throws<ArgumentOutOfRangeException>(
                () => { var twoPlayersGame = new TexasHoldemGame(mockedFirstPlayer.Object, mockedSecondPlayer.Object, initialMoney); });
        }

        [Fact]
        public void ConstructorShouldThrowArgumentOutOfRangeExceptionWhenInitialMoneyAreMoreThan200000()
        {
            var mockedSecondPlayer = new Mock<IPlayer>();
            var mockedFirstPlayer = new Mock<IPlayer>();
            var initialMoney = 200001;
            Assert.Throws<ArgumentOutOfRangeException>(
                () => { var twoPlayersGame = new TexasHoldemGame(mockedFirstPlayer.Object, mockedSecondPlayer.Object, initialMoney); });
        }

        [Fact]
        public void ConstructorShouldThrowArgumentOutOfRangeExceptionWhenInitialMoneyAreZero()
        {
            var mockedSecondPlayer = new Mock<IPlayer>();
            var mockedFirstPlayer = new Mock<IPlayer>();
            var initialMoney = 0;
            Assert.Throws<ArgumentOutOfRangeException>(
                () => { var twoPlayersGame = new TexasHoldemGame(mockedFirstPlayer.Object, mockedSecondPlayer.Object, initialMoney); });
        }

        [Fact]
        public void ConstructorShouldThrowArgumentExceptionWhenTwoPlayersHaveEqualNames()
        {
            var mockedFirstPlayer = new Mock<IPlayer>();
            var mockedSecondPlayer = new Mock<IPlayer>();
            mockedSecondPlayer.SetupGet(x => x.Name).Returns("Player");
            mockedFirstPlayer.SetupGet(x => x.Name).Returns("Player");
            var initialMoney = 1000;
            Assert.Throws<ArgumentException>(
                () => { var twoPlayersGame = new TexasHoldemGame(mockedFirstPlayer.Object, mockedSecondPlayer.Object, initialMoney); });
        }
    }
}

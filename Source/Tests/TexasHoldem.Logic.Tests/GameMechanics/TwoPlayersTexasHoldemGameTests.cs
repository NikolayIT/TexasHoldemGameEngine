namespace TexasHoldem.Logic.Tests.GameMechanics
{
    using System;

    using Moq;
    using NUnit.Framework;
    using TexasHoldem.Logic.GameMechanics;
    using TexasHoldem.Logic.Players;

    [TestFixture]
    public class TwoPlayersTexasHoldemGameTests
    {
        [Test]
        public void ConstructorShouldThrowArgumentNullExceptionWhenIncorrectFirstPlayer()
        {
            IPlayer firstPlayer = null;
            Mock<IPlayer> mockedSecondPlayer = new Mock<IPlayer>();
            var initialMoney = 1000;
            Assert.Throws<ArgumentNullException>(() => new TwoPlayersTexasHoldemGame(firstPlayer, mockedSecondPlayer.Object, initialMoney));
        }

        [Test]
        public void ConstructorShouldThrowArgumentNullExceptionWhenIncorrectSecondPlayer()
        {
            IPlayer secondPlayer = null;
            Mock<IPlayer> mockedFirstPlayer = new Mock<IPlayer>();
            var initialMoney = 1000;
            Assert.Throws<ArgumentNullException>(() => new TwoPlayersTexasHoldemGame(mockedFirstPlayer.Object, secondPlayer, initialMoney));
        }

        [Test]
        public void ConstructorShouldThrowArgumentOutOfRangeExceptionWhenInitialMoneyAreNegative()
        {
            Mock<IPlayer> mockedSecondPlayer = new Mock<IPlayer>();
            Mock<IPlayer> mockedFirstPlayer = new Mock<IPlayer>();
            var initialMoney = -100;
            Assert.Throws<ArgumentOutOfRangeException>(() => new TwoPlayersTexasHoldemGame(mockedFirstPlayer.Object, mockedSecondPlayer.Object, initialMoney));
        }

        [Test]
        public void ConstructorShouldThrowArgumentOutOfRangeExceptionWhenInitialMoneyAreMoreThan200000()
        {
            Mock<IPlayer> mockedSecondPlayer = new Mock<IPlayer>();
            Mock<IPlayer> mockedFirstPlayer = new Mock<IPlayer>();
            var initialMoney = 200001;
            Assert.Throws<ArgumentOutOfRangeException>(() => new TwoPlayersTexasHoldemGame(mockedFirstPlayer.Object, mockedSecondPlayer.Object, initialMoney));
        }

        [Test]
        public void ConstructorShouldThrowArgumentOutOfRangeExceptionWhenInitialMoneyAreZero()
        {
            Mock<IPlayer> mockedSecondPlayer = new Mock<IPlayer>();
            Mock<IPlayer> mockedFirstPlayer = new Mock<IPlayer>();
            var initialMoney = 0;
            Assert.Throws<ArgumentOutOfRangeException>(() => new TwoPlayersTexasHoldemGame(mockedFirstPlayer.Object, mockedSecondPlayer.Object, initialMoney));
        }

        [Test]
        public void ConstructorShouldThrowArgumentExceptionWhenTwoPlayersHaveEqualNames()
        {
            Mock<IPlayer> mockedFirstPlayer = new Mock<IPlayer>();
            Mock<IPlayer> mockedSecondPlayer = new Mock<IPlayer>();
            mockedSecondPlayer.SetupGet(x => x.Name).Returns("Player");
            mockedFirstPlayer.SetupGet(x => x.Name).Returns("Player");
            var initialMoney = 1000;
            Assert.Throws<ArgumentException>(() => new TwoPlayersTexasHoldemGame(mockedFirstPlayer.Object, mockedSecondPlayer.Object, initialMoney));
        }
    }
}

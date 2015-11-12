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
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorShouldThrowArgumentNullExceptionWhenIncorrectFirstPlayer()
        {
            IPlayer firstPlayer = null;
            Mock<IPlayer> mockedSecondPlayer = new Mock<IPlayer>();
            var initialMoney = 1000;
            var twoPlayersGame = new TwoPlayersTexasHoldemGame(firstPlayer, mockedSecondPlayer.Object, initialMoney);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorShouldThrowArgumentNullExceptionWhenIncorrectSecondPlayer()
        {
            IPlayer secondPlayer = null;
            Mock<IPlayer> mockedFirstPlayer = new Mock<IPlayer>();
            var initialMoney = 1000;
            var twoPlayersGame = new TwoPlayersTexasHoldemGame(mockedFirstPlayer.Object, secondPlayer, initialMoney);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ConstructorShouldThrowArgumentOutOfRangeExceptionWhenInitialMoneyAreNegative()
        {
            Mock<IPlayer> mockedSecondPlayer = new Mock<IPlayer>();
            Mock<IPlayer> mockedFirstPlayer = new Mock<IPlayer>();
            var initialMoney = -100;
            var twoPlayersGame = new TwoPlayersTexasHoldemGame(mockedFirstPlayer.Object, mockedSecondPlayer.Object, initialMoney);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ConstructorShouldThrowArgumentOutOfRangeExceptionWhenInitialMoneyAreMoreThan200000()
        {
            Mock<IPlayer> mockedSecondPlayer = new Mock<IPlayer>();
            Mock<IPlayer> mockedFirstPlayer = new Mock<IPlayer>();
            var initialMoney = 200001;
            var twoPlayersGame = new TwoPlayersTexasHoldemGame(mockedFirstPlayer.Object, mockedSecondPlayer.Object, initialMoney);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ConstructorShouldThrowArgumentOutOfRangeExceptionWhenInitialMoneyAreZero()
        {
            Mock<IPlayer> mockedSecondPlayer = new Mock<IPlayer>();
            Mock<IPlayer> mockedFirstPlayer = new Mock<IPlayer>();
            var initialMoney = 0;
            var twoPlayersGame = new TwoPlayersTexasHoldemGame(mockedFirstPlayer.Object, mockedSecondPlayer.Object, initialMoney);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorShouldThrowArgumentExceptionWhenTwoPlayersHaveEqualNames()
        {
            Mock<IPlayer> mockedFirstPlayer = new Mock<IPlayer>();
            Mock<IPlayer> mockedSecondPlayer = new Mock<IPlayer>();
            mockedSecondPlayer.SetupGet(x => x.Name).Returns("Player");
            mockedFirstPlayer.SetupGet(x => x.Name).Returns("Player");
            var initialMoney = 1000;
            var twoPlayersGame = new TwoPlayersTexasHoldemGame(mockedFirstPlayer.Object, mockedSecondPlayer.Object, initialMoney);
        }
    }
}

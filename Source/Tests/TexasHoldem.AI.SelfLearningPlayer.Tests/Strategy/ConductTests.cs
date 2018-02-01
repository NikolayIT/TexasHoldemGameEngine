namespace TexasHoldem.AI.SelfLearningPlayer.Tests.Strategy
{
    using System.Collections.Generic;

    using Moq;
    using NUnit.Framework;
    using TexasHoldem.AI.SelfLearningPlayer.PokerMath;
    using TexasHoldem.AI.SelfLearningPlayer.Strategy;
    using TexasHoldem.Logic.Players;

    [TestFixture]
    public class ConductTests
    {
        [Test]
        public void RelativePositionShouldReturnTheCorrectValue()
        {
            var mockedPocket = new Mock<IPocket>();
            var mockedTurnContext = new Mock<IGetTurnContext>();
            var mockedCalculator = new Mock<ICalculator>();
            var mockedPlayingStyle = new Mock<IPlayingStyle>();

            var opponents = new[]
            {
                new Opponent("player1", 0, null, false, 0),
                new Opponent("player2", 1, null, true, 0),
                new Opponent("player4", 3, null, false, 0),
                new Opponent("player5", 4, null, true, 0),
                new Opponent("player6", 5, null, false, 0)
            };
            mockedTurnContext.SetupGet(x => x.Opponents).Returns(opponents);
            mockedTurnContext.SetupGet(x => x.Position).Returns(2);

            var conduct = new Conduct(
                mockedPocket.Object, mockedTurnContext.Object, mockedCalculator.Object, mockedPlayingStyle.Object);
            Assert.AreEqual(1, conduct.RelativePosition());
        }
    }
}

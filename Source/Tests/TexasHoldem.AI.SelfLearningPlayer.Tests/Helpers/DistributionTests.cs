namespace TexasHoldem.AI.SelfLearningPlayer.Tests.Helpers
{
    using NUnit.Framework;
    using TexasHoldem.AI.SelfLearningPlayer.Helpers;

    [TestFixture]
    public class DistributionTests
    {
        [Test]
        public void FrequencyOfActionFromASpecificPositionShouldReturnAValueEqualToTheRange()
        {
            var numberOfOpponents = 5;
            var sum = new[] { 0.0, 0.0, 0.0 };
            for (int i = 0; i < numberOfOpponents + 1; i++)
            {
                sum[0] += Distribution.FrequencyOfActionFromASpecificPosition(i, numberOfOpponents, 0.33, 0.3);
                sum[1] += Distribution.FrequencyOfActionFromASpecificPosition(i, numberOfOpponents, 0.04, 0.7);
                sum[2] += Distribution.FrequencyOfActionFromASpecificPosition(i, numberOfOpponents, 0.41, 0.2);
            }

            Assert.AreEqual(0.33, sum[0], 0.001);
            Assert.AreEqual(0.04, sum[1], 0.001);
            Assert.AreEqual(0.41, sum[2], 0.001);
        }
    }
}

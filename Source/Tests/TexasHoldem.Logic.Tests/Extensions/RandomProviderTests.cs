namespace TexasHoldem.Logic.Tests.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using TexasHoldem.Logic.Extensions;

    [TestFixture]
    public class RandomProviderTests
    {
        [Test]
        public void NextWithParametersWithParameters1And2ShouldReturn1()
        {
            var value = RandomProvider.Next(1, 2);
            Assert.AreEqual(1, value);
        }

        [Test]
        public void NextWithParametersWithLimitedBoundariesShouldReturnCorrectValue()
        {
            var value = RandomProvider.Next(1337, 1338);
            Assert.AreEqual(1337, value);
        }

        [Test]
        public void NextShouldReturnRandomValues()
        {
            var randomNumbers = new Dictionary<int, int>();
            for (var i = 0; i < 100000; i++)
            {
                var value = RandomProvider.Next(1, 101);
                if (!randomNumbers.ContainsKey(value))
                {
                    randomNumbers.Add(value, 0);
                }

                randomNumbers[value]++;
            }

            Assert.IsFalse(randomNumbers.ContainsKey(0));
            Assert.IsTrue(randomNumbers[1] > 0);
            Assert.IsTrue(randomNumbers[100] > 0);
            Assert.IsFalse(randomNumbers.ContainsKey(101));

            var difference = randomNumbers.Values.Max() - randomNumbers.Values.Min();
            Assert.IsTrue(difference < 2000);
        }
    }
}

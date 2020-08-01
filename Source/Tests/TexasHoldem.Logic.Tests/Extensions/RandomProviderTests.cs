namespace TexasHoldem.Logic.Tests.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Extensions;

    using Xunit;

    public class RandomProviderTests
    {
        [Fact]
        public void NextWithParametersWithParameters1And2ShouldReturn1()
        {
            var value = RandomProvider.Next(1, 2);
            Assert.Equal(1, value);
        }

        [Fact]
        public void NextWithParametersWithLimitedBoundariesShouldReturnCorrectValue()
        {
            var value = RandomProvider.Next(1337, 1338);
            Assert.Equal(1337, value);
        }

        [Fact ]
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

            Assert.False(randomNumbers.ContainsKey(0));
            Assert.True(randomNumbers[1] > 0);
            Assert.True(randomNumbers[100] > 0);
            Assert.False(randomNumbers.ContainsKey(101));

            var difference = randomNumbers.Values.Max() - randomNumbers.Values.Min();
            Assert.True(difference < 2000);
        }
    }
}

namespace TexasHoldem.Logic.Tests.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using TexasHoldem.Logic.Extensions;

    using Xunit;

    public class EnumerableExtensionsTests
    {
        [Fact]
        public void ShuffleShouldThrowAnExceptionWhenCalledOnNull()
        {
            IEnumerable<object> collection = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            Assert.Throws<ArgumentNullException>(() => collection.Shuffle());
        }

        [Fact]
        public void ShuffleShouldReturnAllOfTheGivenElements()
        {
            var list = new List<int> { int.MinValue, -1, 0, 1, 100, int.MaxValue };
            var shuffledList = list.Shuffle().ToList();
            Assert.Equal(list.Count, shuffledList.Count);
            foreach (var item in list)
            {
                Assert.Contains(item, shuffledList);
            }
        }

        [Fact]
        public void ShuffleShouldNotChangeOriginalObjectItemsOrder()
        {
            var list = new List<int> { int.MinValue, -1, 0, 1, 100, int.MaxValue };
            list.Shuffle();
            Assert.Equal(int.MinValue, list[0]);
            Assert.Equal(-1, list[1]);
            Assert.Equal(0, list[2]);
            Assert.Equal(1, list[3]);
            Assert.Equal(100, list[4]);
            Assert.Equal(int.MaxValue, list[5]);
        }

        [Fact]
        public void ShuffleShouldRandomizeOrderOfItems()
        {
            const int NumberOfItems = 100;
            var list = Enumerable.Range(0, NumberOfItems).ToList();
            var shuffledList = list.Shuffle().ToList();
            var equalNumbers = 0;
            for (var i = 0; i < NumberOfItems; i++)
            {
                if (list[i] == shuffledList[i])
                {
                    equalNumbers++;
                }
            }

            Debug.Write($"Equal numbers {equalNumbers}");
            Assert.True(equalNumbers < NumberOfItems / 2);
        }
    }
}

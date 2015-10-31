namespace TexasHoldem.Logic.Tests.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using NUnit.Framework;

    using TexasHoldem.Logic.Extensions;

    [TestFixture]
    public class EnumerableExtensionsTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShuffleShouldThrowAnExceptionWhenCalledOnNull()
        {
            IEnumerable<object> collection = null;

            // ReSharper disable once ExpressionIsAlwaysNull
            collection.Shuffle();
        }

        [Test]
        public void ShuffleShouldReturnAllOfTheGivenElements()
        {
            var list = new List<int> { int.MinValue, -1, 0, 1, 100, int.MaxValue };
            var shuffledList = list.Shuffle().ToList();
            Assert.AreEqual(list.Count, shuffledList.Count);
            foreach (var item in list)
            {
                Assert.IsTrue(shuffledList.Contains(item));
            }
        }

        [Test]
        public void ShuffleShouldNotChangeOriginalObjectItemsOrder()
        {
            var list = new List<int> { int.MinValue, -1, 0, 1, 100, int.MaxValue };
            list.Shuffle();
            Assert.AreEqual(int.MinValue, list[0]);
            Assert.AreEqual(-1, list[1]);
            Assert.AreEqual(0, list[2]);
            Assert.AreEqual(1, list[3]);
            Assert.AreEqual(100, list[4]);
            Assert.AreEqual(int.MaxValue, list[5]);
        }

        [Test]
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
            Assert.IsTrue(equalNumbers < NumberOfItems / 2);
        }
    }
}

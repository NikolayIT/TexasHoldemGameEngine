namespace TexasHoldem.Logic.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Xunit;

    public static class CollectionsAssert
    {
        public static void SameElements<T>(IEnumerable<T> expected, IEnumerable<T> actual)
        {
            Assert.True(!expected.Except(actual).Any() && expected.Count() == actual.Count());
        }
    }
}

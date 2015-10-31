namespace TexasHoldem.Logic.Tests.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Helpers;

    [TestFixture]
    public class BestHandTests
    {
        private static readonly object[] DifferentHandRankTypesCases =
            {
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Seven, CardType.Six,
                                CardType.Five
                            },
                        HandRankType.Pair,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.Seven, CardType.Six,
                                CardType.Five
                            }
                    },
                new object[]
                    {
                        ExpectedCompareResult.SecondShouldBeBetter,
                        HandRankType.Straight,
                        new[]
                            {
                                CardType.King, CardType.Queen,
                                CardType.Jack, CardType.Ten,
                                CardType.Nine
                            },
                        HandRankType.Flush,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Nine
                            }
                    }
            };

        private static readonly object[] BothHaveHighCardCases =
            {
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.HighCard,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Nine
                            },
                        HandRankType.HighCard,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Nine,
                            }
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.HighCard,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Nine
                            },
                        HandRankType.HighCard,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Eight,
                            }
                    }
            };

        private static readonly object[] BothHavePairCases =
            {
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.Pair,
                        new[]
                            {
                                CardType.Ace, CardType.King, CardType.King,
                                CardType.Queen, CardType.Jack
                            },
                        HandRankType.Pair,
                        new[]
                            {
                                CardType.Ace, CardType.King, CardType.King,
                                CardType.Queen, CardType.Jack,
                            }
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.Pair,
                        new[]
                            {
                                CardType.Ace, CardType.Ace, CardType.Four,
                                CardType.Three, CardType.Two
                            },
                        HandRankType.Pair,
                        new[]
                            {
                                CardType.Two, CardType.Two, CardType.Ace,
                                CardType.King, CardType.Jack
                            }
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.Pair,
                        new[]
                            {
                                CardType.Ace, CardType.King, CardType.King,
                                CardType.Queen, CardType.Jack
                            },
                        HandRankType.Pair,
                        new[]
                            {
                                CardType.Ace, CardType.King, CardType.King,
                                CardType.Queen, CardType.Ten
                            }
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.Pair,
                        new[]
                            {
                                CardType.King, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Three,
                            },
                        HandRankType.Pair,
                        new[]
                            {
                                CardType.King, CardType.King,
                                CardType.Queen, CardType.Jack, CardType.Two,
                            }
                    }
            };

        private static readonly object[] BothHaveTwoPairsCases =
            {
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.King, CardType.King, CardType.Queen,
                            },
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.King, CardType.King, CardType.Queen,
                            }
                    },
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.King, CardType.Queen, CardType.Queen,
                            },
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.King, CardType.Queen, CardType.Queen,
                            }
                    },
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.King, CardType.Queen, CardType.Queen,
                            },
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.King, CardType.Queen, CardType.Queen,
                            }
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.King, CardType.King,
                                CardType.Eight, CardType.Eight, CardType.Two
                            },
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Queen, CardType.Queen,
                                CardType.Eight, CardType.Eight, CardType.Two
                            }
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.King, CardType.King,
                                CardType.Seven, CardType.Seven, CardType.Two
                            },
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Queen, CardType.Queen,
                                CardType.Jack, CardType.Jack, CardType.Two
                            }
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.King, CardType.King, CardType.Two
                            },
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.Queen, CardType.Queen, CardType.Two
                            }
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.King, CardType.King, CardType.Queen
                            },
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.King, CardType.King, CardType.Jack
                            }
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.Queen,
                                CardType.Queen, CardType.Jack, CardType.Jack
                            },
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.King, CardType.Queen,
                                CardType.Queen, CardType.Jack, CardType.Jack
                            }
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.Queen, CardType.Two, CardType.Two
                            },
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.Seven, CardType.Two, CardType.Two,
                            }
                    }
            };

        private static readonly object[] BothHaveThreeOfAKindCases =
            {
                //// new object[]
                ////     {
                ////         ExpectedCompareResult.TheyShouldBeEqual,
                ////         HandRankType.ThreeOfAKind,
                ////         new CardType[] { },
                ////         HandRankType.ThreeOfAKind,
                ////         new CardType[] { }
                ////     }
            };

        private static readonly object[] BothHaveStraightCases =
            {
                //// new object[]
                ////     {
                ////         ExpectedCompareResult.TheyShouldBeEqual,
                ////         HandRankType.Straight, new CardType[] { },
                ////         HandRankType.Straight, new CardType[] { }
                ////     }
            };

        private static readonly object[] BothHaveFlushCases =
            {
                //// new object[]
                ////     {
                ////         ExpectedCompareResult.TheyShouldBeEqual,
                ////         HandRankType.Flush, new CardType[] { },
                ////         HandRankType.Flush, new CardType[] { }
                ////     }
            };

        private static readonly object[] BothHaveFullHouseCases =
            {
                //// new object[]
                ////     {
                ////         ExpectedCompareResult.TheyShouldBeEqual,
                ////         HandRankType.FullHouse, new CardType[] { },
                ////         HandRankType.FullHouse, new CardType[] { }
                ////     }
            };

        private static readonly object[] BothHaveFourOfAKindCases =
            {
                //// new object[]
                ////     {
                ////         ExpectedCompareResult.TheyShouldBeEqual,
                ////         HandRankType.FourOfAKind,
                ////         new CardType[] { },
                ////         HandRankType.FourOfAKind,
                ////         new CardType[] { }
                ////     }
            };

        private static readonly object[] BothHaveStraightFlushCases =
            {
                //// new object[]
                ////     {
                ////         ExpectedCompareResult.TheyShouldBeEqual,
                ////         HandRankType.StraightFlush,
                ////         new CardType[] { },
                ////         HandRankType.StraightFlush,
                ////         new CardType[] { }
                ////     }
            };

        public enum ExpectedCompareResult
        {
            FirstShouldBeBetter = 1,
            TheyShouldBeEqual = 0,
            SecondShouldBeBetter = -1,
        }

        [Test]
        [TestCaseSource(nameof(DifferentHandRankTypesCases))]
        [TestCaseSource(nameof(BothHaveHighCardCases))]
        [TestCaseSource(nameof(BothHavePairCases))]
        [TestCaseSource(nameof(BothHaveTwoPairsCases))]
        [TestCaseSource(nameof(BothHaveThreeOfAKindCases))]
        [TestCaseSource(nameof(BothHaveStraightCases))]
        [TestCaseSource(nameof(BothHaveFlushCases))]
        [TestCaseSource(nameof(BothHaveFullHouseCases))]
        [TestCaseSource(nameof(BothHaveFourOfAKindCases))]
        [TestCaseSource(nameof(BothHaveStraightFlushCases))]
        public void CompareToShouldWorkCorrectly(
            ExpectedCompareResult expectedCompareResult,
            HandRankType firstHandRankType,
            IList<CardType> firstCardTypes,
            HandRankType secondHandRankType,
            IList<CardType> secondCardTypes)
        {
            var firstBestHand = new BestHand(firstHandRankType, firstCardTypes.Shuffle().ToList());
            var secondBestHand = new BestHand(secondHandRankType, secondCardTypes.Shuffle().ToList());
            var compareToResultFirstSecond = firstBestHand.CompareTo(secondBestHand);
            var compareToResultSecondFirst = secondBestHand.CompareTo(firstBestHand);
            switch (expectedCompareResult)
            {
                case ExpectedCompareResult.FirstShouldBeBetter:
                    Assert.IsTrue(compareToResultFirstSecond > 0, "compareToResultFirstSecond > 0");
                    Assert.IsTrue(compareToResultSecondFirst < 0, "compareToResultSecondFirst < 0");
                    break;
                case ExpectedCompareResult.SecondShouldBeBetter:
                    Assert.IsTrue(compareToResultFirstSecond < 0, "compareToResultFirstSecond < 0");
                    Assert.IsTrue(compareToResultSecondFirst > 0, "compareToResultSecondFirst > 0");
                    break;
                case ExpectedCompareResult.TheyShouldBeEqual:
                    Assert.AreEqual(0, compareToResultFirstSecond);
                    Assert.AreEqual(0, compareToResultSecondFirst);
                    break;
                default:
                    Assert.Fail("Invalid ExpectedCompareResult value");
                    break;
            }
        }

        [Test]
        public void ConstructorSetsProperties()
        {
            var rankType = HandRankType.Straight;
            var cardTypes = new List<CardType>
                                {
                                    CardType.Ace,
                                    CardType.Three,
                                    CardType.Four,
                                    CardType.Five,
                                    CardType.Two
                                };
            var bestHand = new BestHand(rankType, cardTypes);
            Assert.AreEqual(rankType, bestHand.RankType);
            CollectionAssert.AreEquivalent(cardTypes, bestHand.Cards);
        }
    }
}

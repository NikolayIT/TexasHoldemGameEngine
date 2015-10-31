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
                        ExpectedCompareResult
                            .FirstShouldBeBetter,
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
                        ExpectedCompareResult
                            .SecondShouldBeBetter,
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
                    },
                new object[]
                    {
                        ExpectedCompareResult.SecondShouldBeBetter,
                        HandRankType.HighCard,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Eight
                            },
                        HandRankType.HighCard,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Nine,
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
                        ExpectedCompareResult.SecondShouldBeBetter,
                        HandRankType.Pair,
                        new[]
                            {
                                CardType.Ace, CardType.King, CardType.King,
                                CardType.Queen, CardType.Ten
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
                    },
                new object[]
                    {
                        ExpectedCompareResult.SecondShouldBeBetter,
                        HandRankType.Pair,
                        new[]
                            {
                                CardType.King, CardType.King,
                                CardType.Queen, CardType.Jack, CardType.Two,
                            },
                        HandRankType.Pair,
                        new[]
                            {
                                CardType.King, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Three,
                            }
                    }
            };

        private static readonly object[] BothHaveTwoPairsCases =
            {
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.TwoPairs, new CardType[] { },
                        HandRankType.TwoPairs, new CardType[] { }
                    }
            };

        private static readonly object[] BothHaveThreeOfAKindCases =
            {
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.ThreeOfAKind,
                        new CardType[] { },
                        HandRankType.ThreeOfAKind,
                        new CardType[] { }
                    }
            };

        private static readonly object[] BothHaveStraightCases =
            {
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.Straight, new CardType[] { },
                        HandRankType.Straight, new CardType[] { }
                    }
            };

        private static readonly object[] BothHaveFlushCases =
            {
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.Flush, new CardType[] { },
                        HandRankType.Flush, new CardType[] { }
                    }
            };

        private static readonly object[] BothHaveFullHouseCases =
            {
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.FullHouse, new CardType[] { },
                        HandRankType.FullHouse, new CardType[] { }
                    }
            };

        private static readonly object[] BothHaveFourOfAKindCases =
            {
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.FourOfAKind,
                        new CardType[] { },
                        HandRankType.FourOfAKind,
                        new CardType[] { }
                    }
            };

        private static readonly object[] BothHaveStraightFlushCases =
            {
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.StraightFlush,
                        new CardType[] { },
                        HandRankType.StraightFlush,
                        new CardType[] { }
                    }
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
            ICollection<CardType> firstCardTypes,
            HandRankType secondHandRankType,
            ICollection<CardType> secondCardTypes)
        {
            var firstBestHand = new BestHand(firstHandRankType, firstCardTypes.Shuffle().ToList());
            var secondBestHand = new BestHand(secondHandRankType, secondCardTypes.Shuffle().ToList());
            var compareToResult = firstBestHand.CompareTo(secondBestHand);
            switch (expectedCompareResult)
            {
                case ExpectedCompareResult.FirstShouldBeBetter:
                    Assert.IsTrue(compareToResult > 0, "compareToResult > 0");
                    break;
                case ExpectedCompareResult.SecondShouldBeBetter:
                    Assert.IsTrue(compareToResult < 0, "compareToResult < 0");
                    break;
                case ExpectedCompareResult.TheyShouldBeEqual:
                    Assert.AreEqual(0, compareToResult);
                    break;
                default:
                    Assert.Fail("Invalid ExpectedCompareResult value");
                    break;
            }
        }

        [Test]
        public void ConstructorSetsProperties()
        {
            var rankType = HandRankType.FourOfAKind;
            var cardTypes = new List<CardType> { CardType.Ace, CardType.Two };
            var bestHand = new BestHand(rankType, cardTypes);
            Assert.AreEqual(rankType, bestHand.RankType);
            CollectionAssert.AreEquivalent(cardTypes, bestHand.Cards);
        }
    }
}

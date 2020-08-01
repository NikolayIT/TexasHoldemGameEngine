namespace TexasHoldem.Logic.Tests.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Helpers;

    using Xunit;

    public class BestHandTests
    {
        public static readonly IEnumerable<object[]> DifferentHandRankTypesCases =
            new List<object[]>
            {
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Seven, CardType.Six,
                                CardType.Five,
                            },
                        HandRankType.Pair,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.Seven, CardType.Six,
                                CardType.Five,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.SecondShouldBeBetter,
                        HandRankType.Straight,
                        new[]
                            {
                                CardType.King, CardType.Queen,
                                CardType.Jack, CardType.Ten,
                                CardType.Nine,
                            },
                        HandRankType.Flush,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Nine,
                            },
                    },
            };

        public static readonly IEnumerable<object[]> BothHaveHighCardCases =
            new List<object[]>
            {
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.HighCard,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Nine,
                            },
                        HandRankType.HighCard,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Nine,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.HighCard,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Nine,
                            },
                        HandRankType.HighCard,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Eight,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.HighCard,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Two,
                            },
                        HandRankType.HighCard,
                        new[]
                            {
                                CardType.King, CardType.Queen,
                                CardType.Jack, CardType.Ten,
                                CardType.Three,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.SecondShouldBeBetter,
                        HandRankType.HighCard,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Two,
                            },
                        HandRankType.HighCard,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Three,
                            },
                    },
            };

        public static readonly IEnumerable<object[]> BothHavePairCases =
            new List<object[]>
            {
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.Pair,
                        new[]
                            {
                                CardType.Ace, CardType.King, CardType.King,
                                CardType.Queen, CardType.Jack,
                            },
                        HandRankType.Pair,
                        new[]
                            {
                                CardType.Ace, CardType.King, CardType.King,
                                CardType.Queen, CardType.Jack,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.Pair,
                        new[]
                            {
                                CardType.Ace, CardType.Ace, CardType.Four,
                                CardType.Three, CardType.Two,
                            },
                        HandRankType.Pair,
                        new[]
                            {
                                CardType.Two, CardType.Two, CardType.Ace,
                                CardType.King, CardType.Jack,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.Pair,
                        new[]
                            {
                                CardType.Ace, CardType.King, CardType.King,
                                CardType.Queen, CardType.Jack,
                            },
                        HandRankType.Pair,
                        new[]
                            {
                                CardType.Ace, CardType.King, CardType.King,
                                CardType.Queen, CardType.Ten,
                            },
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
                            },
                    },
            };

        public static readonly IEnumerable<object[]> BothHaveTwoPairsCases =
            new List<object[]>
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
                            },
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
                            },
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
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.King, CardType.King,
                                CardType.Eight, CardType.Eight, CardType.Two,
                            },
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Queen, CardType.Queen,
                                CardType.Eight, CardType.Eight, CardType.Two,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.King, CardType.King,
                                CardType.Seven, CardType.Seven, CardType.Two,
                            },
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Queen, CardType.Queen,
                                CardType.Jack, CardType.Jack, CardType.Two,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.King, CardType.King, CardType.Two,
                            },
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.Queen, CardType.Queen, CardType.Two,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
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
                                CardType.King, CardType.King, CardType.Jack,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.SecondShouldBeBetter,
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.King, CardType.King, CardType.Nine,
                            },
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.King, CardType.King, CardType.Jack,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.Queen,
                                CardType.Queen, CardType.Jack, CardType.Jack,
                            },
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.King, CardType.Queen,
                                CardType.Queen, CardType.Jack, CardType.Jack,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.Queen, CardType.Two, CardType.Two,
                            },
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.Seven, CardType.Two, CardType.Two,
                            },
                    },
            };

        public static readonly IEnumerable<object[]> BothHaveThreeOfAKindCases =
            new List<object[]>
            {
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.Ace, CardType.Five, CardType.Two,
                            },
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.Ace, CardType.Five, CardType.Two,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.King, CardType.King, CardType.Two,
                            },
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.King, CardType.King, CardType.Two,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Queen, CardType.Queen,
                            },
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Queen, CardType.Queen,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.Ace, CardType.Five, CardType.Two,
                            },
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.King, CardType.King,
                                CardType.King, CardType.Five, CardType.Two,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.King, CardType.King, CardType.Two,
                            },
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.Queen,
                                CardType.Queen, CardType.Queen, CardType.Two,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Queen, CardType.Queen,
                            },
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Jack, CardType.Jack, CardType.Jack,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.Ace, CardType.Four, CardType.Three,
                            },
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.Ace, CardType.Four, CardType.Two,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.Ace, CardType.Five, CardType.Two,
                            },
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.Ace, CardType.Four, CardType.Two,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.Queen,
                                CardType.Queen, CardType.Queen, CardType.Two,
                            },
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.King, CardType.Queen,
                                CardType.Queen, CardType.Queen, CardType.Two,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.Queen,
                                CardType.Queen, CardType.Queen, CardType.Three,
                            },
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.Queen,
                                CardType.Queen, CardType.Queen, CardType.Two,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.Jack,
                                CardType.Three, CardType.Three, CardType.Three,
                            },
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.King, CardType.Jack,
                                CardType.Three, CardType.Three, CardType.Three,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.Jack,
                                CardType.Three, CardType.Three, CardType.Three,
                            },
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.Ten,
                                CardType.Three, CardType.Three, CardType.Three,
                            },
                    },
            };

        public static readonly IEnumerable<object[]> BothHaveStraightCases =
            new List<object[]>
            {
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.Straight,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Ten,
                            },
                        HandRankType.Straight,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Ten,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.Straight,
                        new[]
                            {
                                CardType.Six, CardType.Five,
                                CardType.Four, CardType.Three,
                                CardType.Two,
                            },
                        HandRankType.Straight,
                        new[]
                            {
                                CardType.Six, CardType.Five,
                                CardType.Four, CardType.Three,
                                CardType.Two,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.Straight,
                        new[]
                            {
                                CardType.Five, CardType.Four,
                                CardType.Three, CardType.Two,
                                CardType.Ace,
                            },
                        HandRankType.Straight,
                        new[]
                            {
                                CardType.Five, CardType.Four,
                                CardType.Three, CardType.Two,
                                CardType.Ace,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.Straight,
                        new[]
                            {
                                CardType.Six, CardType.Five,
                                CardType.Four, CardType.Three,
                                CardType.Two,
                            },
                        HandRankType.Straight,
                        new[]
                            {
                                CardType.Five, CardType.Four,
                                CardType.Three, CardType.Two,
                                CardType.Ace,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.Straight,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Ten,
                            },
                        HandRankType.Straight,
                        new[]
                            {
                                CardType.Five, CardType.Four,
                                CardType.Three, CardType.Two,
                                CardType.Ace,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.Straight,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Ten,
                            },
                        HandRankType.Straight,
                        new[]
                            {
                                CardType.King, CardType.Queen,
                                CardType.Jack, CardType.Ten,
                                CardType.Nine,
                            },
                    },
            };

        public static readonly IEnumerable<object[]> BothHaveFlushCases =
            new List<object[]>
            {
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.Flush,
                        new[]
                            {
                                CardType.Ace, CardType.King, CardType.Ten,
                                CardType.Eight, CardType.Two,
                            },
                        HandRankType.Flush,
                        new[]
                            {
                                CardType.Ace, CardType.King, CardType.Nine,
                                CardType.Eight, CardType.Five,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.Flush,
                        new[]
                            {
                                CardType.Ace, CardType.King, CardType.Ten,
                                CardType.Eight, CardType.Two,
                            },
                        HandRankType.Flush,
                        new[]
                            {
                                CardType.Ace, CardType.King, CardType.Ten,
                                CardType.Eight, CardType.Two,
                            },
                    },
            };

        public static readonly IEnumerable<object[]> BothHaveFullHouseCases =
            new List<object[]>
            {
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.FullHouse,
                        new[]
                            {
                                CardType.Ace, CardType.Ace, CardType.Ace,
                                CardType.King, CardType.King,
                            },
                        HandRankType.FullHouse,
                        new[]
                            {
                                CardType.Ace, CardType.Ace, CardType.Ace,
                                CardType.King, CardType.King,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.FullHouse,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.King, CardType.King, CardType.King,
                            },
                        HandRankType.FullHouse,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.King, CardType.King, CardType.King,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.FullHouse,
                        new[]
                            {
                                CardType.Three, CardType.Three, CardType.Three,
                                CardType.Ace, CardType.Ace,
                            },
                        HandRankType.FullHouse,
                        new[]
                            {
                                CardType.Two, CardType.Two, CardType.Two,
                                CardType.Ace, CardType.Ace,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.FullHouse,
                        new[]
                            {
                                CardType.Three, CardType.Three, CardType.Three,
                                CardType.Four, CardType.Four,
                            },
                        HandRankType.FullHouse,
                        new[]
                            {
                                CardType.Two, CardType.Two, CardType.Two,
                                CardType.Ace, CardType.Ace,
                            },
                    },
            };

        public static readonly IEnumerable<object[]> BothHaveFourOfAKindCases =
            new List<object[]>
            {
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.FourOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.Ace, CardType.Ace,
                                CardType.King,
                            },
                        HandRankType.FourOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.Ace, CardType.Ace,
                                CardType.King,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.FourOfAKind,
                        new[]
                            {
                                CardType.Three, CardType.Two,
                                CardType.Two, CardType.Two,
                                CardType.Two,
                            },
                        HandRankType.FourOfAKind,
                        new[]
                            {
                                CardType.Three, CardType.Two,
                                CardType.Two, CardType.Two,
                                CardType.Two,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.FourOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.Ace, CardType.Ace,
                                CardType.Jack,
                            },
                        HandRankType.FourOfAKind,
                        new[]
                            {
                                CardType.King, CardType.King,
                                CardType.King, CardType.King,
                                CardType.Queen,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.FourOfAKind,
                        new[]
                            {
                                CardType.Four, CardType.Three,
                                CardType.Three, CardType.Three,
                                CardType.Three,
                            },
                        HandRankType.FourOfAKind,
                        new[]
                            {
                                CardType.Five, CardType.Two,
                                CardType.Two, CardType.Two,
                                CardType.Two,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.FourOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.Ace, CardType.Ace,
                                CardType.King,
                            },
                        HandRankType.FourOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.Ace,
                                CardType.Ace, CardType.Ace,
                                CardType.Queen,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.FourOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.Two,
                                CardType.Two, CardType.Two,
                                CardType.Two,
                            },
                        HandRankType.FourOfAKind,
                        new[]
                            {
                                CardType.King, CardType.Two,
                                CardType.Two, CardType.Two,
                                CardType.Two,
                            },
                    },
            };

        public static readonly IEnumerable<object[]> BothHaveStraightFlushCases =
            new List<object[]>
            {
                new object[]
                    {
                        ExpectedCompareResult.TheyShouldBeEqual,
                        HandRankType.StraightFlush,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Ten,
                            },
                        HandRankType.StraightFlush,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Ten,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.StraightFlush,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Ten,
                            },
                        HandRankType.StraightFlush,
                        new[]
                            {
                                CardType.King, CardType.Queen,
                                CardType.Jack, CardType.Ten,
                                CardType.Nine,
                            },
                    },
                new object[]
                    {
                        ExpectedCompareResult.FirstShouldBeBetter,
                        HandRankType.StraightFlush,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Ten,
                            },
                        HandRankType.StraightFlush,
                        new[]
                            {
                                CardType.Five, CardType.Four,
                                CardType.Three, CardType.Two,
                                CardType.Ace,
                            },
                    },
            };

        public enum ExpectedCompareResult
        {
            FirstShouldBeBetter = 1,
            TheyShouldBeEqual = 0,
            SecondShouldBeBetter = -1,
        }

        [Theory]
        [MemberData(nameof(DifferentHandRankTypesCases))]
        [MemberData(nameof(BothHaveHighCardCases))]
        [MemberData(nameof(BothHavePairCases))]
        [MemberData(nameof(BothHaveTwoPairsCases))]
        [MemberData(nameof(BothHaveThreeOfAKindCases))]
        [MemberData(nameof(BothHaveStraightCases))]
        [MemberData(nameof(BothHaveFlushCases))]
        [MemberData(nameof(BothHaveFullHouseCases))]
        [MemberData(nameof(BothHaveFourOfAKindCases))]
        [MemberData(nameof(BothHaveStraightFlushCases))]
        public void CompareToShouldWorkCorrectly(
            ExpectedCompareResult expectedCompareResult,
            HandRankType firstHandRankType,
            ICollection<CardType> firstCardTypes,
            HandRankType secondHandRankType,
            ICollection<CardType> secondCardTypes)
        {
            var firstBestHand = new BestHand(firstHandRankType, firstCardTypes.Shuffle().ToList());
            var secondBestHand = new BestHand(secondHandRankType, secondCardTypes.Shuffle().ToList());
            var compareToResultFirstSecond = firstBestHand.CompareTo(secondBestHand);
            var compareToResultSecondFirst = secondBestHand.CompareTo(firstBestHand);
            switch (expectedCompareResult)
            {
                case ExpectedCompareResult.FirstShouldBeBetter:
                    Assert.True(compareToResultFirstSecond > 0, "compareToResultFirstSecond > 0");
                    Assert.True(compareToResultSecondFirst < 0, "compareToResultSecondFirst < 0");
                    break;
                case ExpectedCompareResult.SecondShouldBeBetter:
                    Assert.True(compareToResultFirstSecond < 0, "compareToResultFirstSecond < 0");
                    Assert.True(compareToResultSecondFirst > 0, "compareToResultSecondFirst > 0");
                    break;
                case ExpectedCompareResult.TheyShouldBeEqual:
                    Assert.Equal(0, compareToResultFirstSecond);
                    Assert.Equal(0, compareToResultSecondFirst);
                    break;
                default:
                    Assert.True(false, "Invalid ExpectedCompareResult value");
                    break;
            }
        }

        [Fact]
        public void ConstructorSetsProperties()
        {
            var rankType = HandRankType.Straight;
            var cardTypes = new List<CardType>
                                {
                                    CardType.Ace,
                                    CardType.Three,
                                    CardType.Four,
                                    CardType.Five,
                                    CardType.Two,
                                };
            var bestHand = new BestHand(rankType, cardTypes);
            Assert.Equal(rankType, bestHand.RankType);
            Assert.Equal(cardTypes, bestHand.Cards);
        }
    }
}

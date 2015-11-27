namespace TexasHoldem.Logic.Tests.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Helpers;

    public class HandEvaluatorTests
    {
        private static readonly object[] HighCardCases =
            {
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Spade, CardType.King),
                                new Card(CardSuit.Spade, CardType.Seven),
                                new Card(CardSuit.Heart, CardType.Six),
                                new Card(CardSuit.Spade, CardType.Five),
                                new Card(CardSuit.Club, CardType.Three),
                                new Card(CardSuit.Diamond, CardType.Two)
                            },
                        HandRankType.HighCard,
                        new[]
                            {
                                CardType.Ace, CardType.King, CardType.Seven,
                                CardType.Six, CardType.Five
                            }
                    },
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Spade, CardType.King),
                                new Card(CardSuit.Spade, CardType.Queen),
                                new Card(CardSuit.Heart, CardType.Jack),
                                new Card(CardSuit.Spade, CardType.Nine),
                                new Card(CardSuit.Club, CardType.Eight),
                                new Card(CardSuit.Diamond, CardType.Seven),
                                new Card(CardSuit.Diamond, CardType.Six)
                            },
                        HandRankType.HighCard,
                        new[]
                            {
                                CardType.Ace, CardType.King, CardType.Queen,
                                CardType.Jack, CardType.Nine
                            }
                    }
            };

        private static readonly object[] PairCases =
            {
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Spade, CardType.King),
                                new Card(CardSuit.Spade, CardType.Seven),
                                new Card(CardSuit.Heart, CardType.Six),
                                new Card(CardSuit.Spade, CardType.Five),
                                new Card(CardSuit.Club, CardType.Two),
                                new Card(CardSuit.Diamond, CardType.Two)
                            },
                        HandRankType.Pair,
                        new[]
                            {
                                CardType.Two, CardType.Two, CardType.Ace,
                                CardType.King, CardType.Seven
                            }
                    },
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Diamond, CardType.Ace),
                                new Card(CardSuit.Spade, CardType.King),
                                new Card(CardSuit.Heart, CardType.Six),
                                new Card(CardSuit.Spade, CardType.Four),
                                new Card(CardSuit.Club, CardType.Three),
                                new Card(CardSuit.Diamond, CardType.Two)
                            },
                        HandRankType.Pair,
                        new[]
                            {
                                CardType.Ace, CardType.Ace, CardType.King,
                                CardType.Six, CardType.Four
                            }
                    },
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Heart, CardType.Seven),
                                new Card(CardSuit.Diamond, CardType.Seven),
                                new Card(CardSuit.Spade, CardType.Six),
                                new Card(CardSuit.Spade, CardType.Four),
                                new Card(CardSuit.Club, CardType.Three),
                                new Card(CardSuit.Diamond, CardType.Two)
                            },
                        HandRankType.Pair,
                        new[]
                            {
                                CardType.Ace, CardType.Seven, CardType.Seven,
                                CardType.Six, CardType.Four
                            }
                    }
            };

        private static readonly object[] TwoPairsCases =
            {
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Spade, CardType.Seven),
                                new Card(CardSuit.Heart, CardType.Seven),
                                new Card(CardSuit.Heart, CardType.Six),
                                new Card(CardSuit.Spade, CardType.Six),
                                new Card(CardSuit.Club, CardType.Three),
                                new Card(CardSuit.Diamond, CardType.Two)
                            },
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Seven, CardType.Seven, CardType.Six,
                                CardType.Six, CardType.Ace
                            }
                    },
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Heart, CardType.Seven),
                                new Card(CardSuit.Heart, CardType.Six),
                                new Card(CardSuit.Spade, CardType.Six),
                                new Card(CardSuit.Club, CardType.Three),
                                new Card(CardSuit.Diamond, CardType.Three)
                            },
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.Ace, CardType.Seven,
                                CardType.Six, CardType.Six
                            }
                    },
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Heart, CardType.Six),
                                new Card(CardSuit.Spade, CardType.Six),
                                new Card(CardSuit.Heart, CardType.Four),
                                new Card(CardSuit.Club, CardType.Four),
                                new Card(CardSuit.Diamond, CardType.Two)
                            },
                        HandRankType.TwoPairs,
                        new[]
                            {
                                CardType.Ace, CardType.Ace, CardType.Six,
                                CardType.Six, CardType.Four
                            }
                    }
            };

        private static readonly object[] ThreeOfAKindCases =
            {
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Spade, CardType.King),
                                new Card(CardSuit.Spade, CardType.Six),
                                new Card(CardSuit.Heart, CardType.Six),
                                new Card(CardSuit.Club, CardType.Six),
                                new Card(CardSuit.Club, CardType.Three),
                                new Card(CardSuit.Diamond, CardType.Two)
                            },
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Six, CardType.Six, CardType.Six,
                                CardType.Ace, CardType.King
                            }
                    },
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Heart, CardType.Ace),
                                new Card(CardSuit.Diamond, CardType.Ace),
                                new Card(CardSuit.Heart, CardType.King),
                                new Card(CardSuit.Club, CardType.Four),
                                new Card(CardSuit.Club, CardType.Three),
                                new Card(CardSuit.Diamond, CardType.Two)
                            },
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.Ace, CardType.Ace,
                                CardType.King, CardType.Four
                            }
                    },
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Jack),
                                new Card(CardSuit.Heart, CardType.Ten),
                                new Card(CardSuit.Diamond, CardType.Ten),
                                new Card(CardSuit.Heart, CardType.Ten),
                                new Card(CardSuit.Club, CardType.Nine),
                                new Card(CardSuit.Club, CardType.Eight),
                                new Card(CardSuit.Diamond, CardType.Two)
                            },
                        HandRankType.ThreeOfAKind,
                        new[]
                            {
                                CardType.Ten, CardType.Ten, CardType.Ten,
                                CardType.Jack, CardType.Nine
                            }
                    }
            };

        private static readonly object[] StraightCases =
            {
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Spade, CardType.Two),
                                new Card(CardSuit.Spade, CardType.Three),
                                new Card(CardSuit.Heart, CardType.Four),
                                new Card(CardSuit.Spade, CardType.Five),
                                new Card(CardSuit.Club, CardType.Jack),
                                new Card(CardSuit.Diamond, CardType.Queen)
                            },
                        HandRankType.Straight,
                        new[]
                        {
                            CardType.Ace, CardType.Two, CardType.Three,
                            CardType.Four, CardType.Five
                        }
                    },
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Spade, CardType.King),
                                new Card(CardSuit.Spade, CardType.Queen),
                                new Card(CardSuit.Heart, CardType.Jack),
                                new Card(CardSuit.Spade, CardType.Ten),
                                new Card(CardSuit.Club, CardType.Seven),
                                new Card(CardSuit.Diamond, CardType.Three)
                            },
                        HandRankType.Straight,
                        new[]
                        {
                            CardType.Ace, CardType.King, CardType.Queen,
                            CardType.Jack, CardType.Ten
                        }
                    },
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Spade, CardType.Two),
                                new Card(CardSuit.Spade, CardType.Queen),
                                new Card(CardSuit.Heart, CardType.Jack),
                                new Card(CardSuit.Spade, CardType.Ten),
                                new Card(CardSuit.Club, CardType.Nine),
                                new Card(CardSuit.Diamond, CardType.Eight)
                            },
                        HandRankType.Straight,
                        new[]
                        {
                            CardType.Queen, CardType.Jack, CardType.Ten,
                            CardType.Nine, CardType.Eight
                        }
                    },
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Diamond, CardType.Ace),
                                new Card(CardSuit.Heart, CardType.Two),
                                new Card(CardSuit.Diamond, CardType.Three),
                                new Card(CardSuit.Heart, CardType.Four),
                                new Card(CardSuit.Diamond, CardType.Five),
                                new Card(CardSuit.Heart, CardType.Six),
                                new Card(CardSuit.Diamond, CardType.Seven)
                            },
                        HandRankType.Straight,
                        new[]
                        {
                            CardType.Three, CardType.Four, CardType.Five,
                            CardType.Six, CardType.Seven
                        }
                    },
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Diamond, CardType.Three),
                                new Card(CardSuit.Heart, CardType.Four),
                                new Card(CardSuit.Diamond, CardType.Five),
                                new Card(CardSuit.Heart, CardType.Six),
                                new Card(CardSuit.Heart, CardType.Six),
                                new Card(CardSuit.Diamond, CardType.Seven),
                                new Card(CardSuit.Diamond, CardType.Seven)
                            },
                        HandRankType.Straight,
                        new[]
                        {
                            CardType.Three, CardType.Four, CardType.Five,
                            CardType.Six, CardType.Seven
                        }
                    },
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Heart, CardType.Three),
                                new Card(CardSuit.Diamond, CardType.Three),
                                new Card(CardSuit.Heart, CardType.Four),
                                new Card(CardSuit.Diamond, CardType.Five),
                                new Card(CardSuit.Heart, CardType.Six),
                                new Card(CardSuit.Diamond, CardType.Seven),
                                new Card(CardSuit.Heart, CardType.Seven)
                            },
                        HandRankType.Straight,
                        new[]
                        {
                            CardType.Three, CardType.Four, CardType.Five,
                            CardType.Six, CardType.Seven
                        }
                    }
            };

        private static readonly object[] FlushCases =
            {
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Spade, CardType.King),
                                new Card(CardSuit.Spade, CardType.Queen),
                                new Card(CardSuit.Heart, CardType.Jack),
                                new Card(CardSuit.Spade, CardType.Ten),
                                new Card(CardSuit.Club, CardType.Seven),
                                new Card(CardSuit.Spade, CardType.Three)
                            },
                        HandRankType.Flush,
                        new[]
                        {
                            CardType.Ace, CardType.King, CardType.Queen,
                            CardType.Ten, CardType.Three
                        }
                    },
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Spade, CardType.King),
                                new Card(CardSuit.Spade, CardType.Queen),
                                new Card(CardSuit.Heart, CardType.Ten),
                                new Card(CardSuit.Spade, CardType.Nine),
                                new Card(CardSuit.Club, CardType.Seven),
                                new Card(CardSuit.Spade, CardType.Three)
                            },
                        HandRankType.Flush,
                        new[]
                        {
                            CardType.Ace, CardType.King, CardType.Queen,
                            CardType.Nine, CardType.Three
                        }
                    },
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Spade, CardType.King),
                                new Card(CardSuit.Spade, CardType.Queen),
                                new Card(CardSuit.Spade, CardType.Ten),
                                new Card(CardSuit.Spade, CardType.Nine),
                                new Card(CardSuit.Spade, CardType.Seven),
                                new Card(CardSuit.Spade, CardType.Three)
                            },
                        HandRankType.Flush,
                        new[]
                        {
                            CardType.Ace, CardType.King, CardType.Queen,
                            CardType.Ten, CardType.Nine
                        }
                    },
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Spade, CardType.King),
                                new Card(CardSuit.Heart, CardType.Queen),
                                new Card(CardSuit.Spade, CardType.Jack),
                                new Card(CardSuit.Spade, CardType.Ten),
                                new Card(CardSuit.Spade, CardType.Seven),
                                new Card(CardSuit.Spade, CardType.Three)
                            },
                        HandRankType.Flush,
                        new[]
                        {
                            CardType.Ace, CardType.King, CardType.Jack,
                            CardType.Ten, CardType.Seven
                        }
                    }
            };

        private static readonly object[] FullHouseCases =
            {
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Heart, CardType.Ace),
                                new Card(CardSuit.Club, CardType.Ace),
                                new Card(CardSuit.Diamond, CardType.Ten),
                                new Card(CardSuit.Club, CardType.Ten),
                                new Card(CardSuit.Club, CardType.Seven),
                                new Card(CardSuit.Spade, CardType.Three)
                            },
                        HandRankType.FullHouse,
                        new[]
                        {
                            CardType.Ace, CardType.Ace, CardType.Ace,
                            CardType.Ten, CardType.Ten
                        }
                    },
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Two),
                                new Card(CardSuit.Heart, CardType.Two),
                                new Card(CardSuit.Club, CardType.Two),
                                new Card(CardSuit.Diamond, CardType.Ten),
                                new Card(CardSuit.Club, CardType.Ten),
                                new Card(CardSuit.Club, CardType.Seven),
                                new Card(CardSuit.Spade, CardType.Seven)
                            },
                        HandRankType.FullHouse,
                        new[]
                        {
                            CardType.Two, CardType.Two, CardType.Two,
                            CardType.Ten, CardType.Ten
                        }
                    },
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Heart, CardType.Ace),
                                new Card(CardSuit.Club, CardType.Ace),
                                new Card(CardSuit.Diamond, CardType.King),
                                new Card(CardSuit.Club, CardType.King),
                                new Card(CardSuit.Heart, CardType.King),
                                new Card(CardSuit.Spade, CardType.Queen)
                            },
                        HandRankType.FullHouse,
                        new[]
                        {
                            CardType.Ace, CardType.Ace, CardType.Ace,
                            CardType.King, CardType.King
                        }
                    }
            };

        private static readonly object[] FourOfAKindCases =
            {
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Heart, CardType.Ace),
                                new Card(CardSuit.Club, CardType.Ace),
                                new Card(CardSuit.Diamond, CardType.Ace),
                                new Card(CardSuit.Club, CardType.Ten),
                                new Card(CardSuit.Club, CardType.Seven),
                                new Card(CardSuit.Spade, CardType.Three)
                            },
                        HandRankType.FourOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.Ace, CardType.Ace,
                                CardType.Ace, CardType.Ten
                            }
                    },
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Heart, CardType.Ace),
                                new Card(CardSuit.Club, CardType.Ace),
                                new Card(CardSuit.Club, CardType.Ace),
                                new Card(CardSuit.Diamond, CardType.Two),
                                new Card(CardSuit.Club, CardType.Two),
                                new Card(CardSuit.Club, CardType.Two),
                                new Card(CardSuit.Spade, CardType.Two)
                            },
                        HandRankType.FourOfAKind,
                        new[]
                            {
                                CardType.Ace, CardType.Ace, CardType.Ace,
                                CardType.Ace, CardType.Two
                            }
                    },
            };

        private static readonly object[] StraightFlushCases =
            {
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Spade, CardType.Two),
                                new Card(CardSuit.Spade, CardType.Three),
                                new Card(CardSuit.Spade, CardType.Four),
                                new Card(CardSuit.Spade, CardType.Five),
                                new Card(
                                    CardSuit.Diamond,
                                    CardType.Eight),
                                new Card(CardSuit.Heart, CardType.Jack),
                                new Card(CardSuit.Club, CardType.Queen)
                            },
                        HandRankType.StraightFlush,
                        new[]
                            {
                                CardType.Ace, CardType.Two,
                                CardType.Three, CardType.Four,
                                CardType.Five
                            }
                    },
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Spade, CardType.Two),
                                new Card(CardSuit.Spade, CardType.Three),
                                new Card(CardSuit.Spade, CardType.Four),
                                new Card(CardSuit.Spade, CardType.Five),
                                new Card(CardSuit.Spade, CardType.Eight),
                                new Card(CardSuit.Spade, CardType.Jack),
                                new Card(CardSuit.Spade, CardType.Queen)
                            },
                        HandRankType.StraightFlush,
                        new[]
                            {
                                CardType.Ace, CardType.Two,
                                CardType.Three, CardType.Four,
                                CardType.Five
                            }
                    },
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Spade, CardType.Two),
                                new Card(CardSuit.Spade, CardType.Three),
                                new Card(CardSuit.Spade, CardType.Four),
                                new Card(CardSuit.Spade, CardType.Five),
                                new Card(CardSuit.Spade, CardType.Six),
                                new Card(CardSuit.Spade, CardType.Seven)
                            },
                        HandRankType.StraightFlush,
                        new[]
                            {
                                CardType.Three, CardType.Four,
                                CardType.Five, CardType.Six,
                                CardType.Seven
                            }
                    },
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Spade, CardType.Two),
                                new Card(CardSuit.Spade, CardType.Three),
                                new Card(CardSuit.Spade, CardType.Four),
                                new Card(CardSuit.Spade, CardType.Five),
                                new Card(CardSuit.Spade, CardType.Ten),
                                new Card(CardSuit.Spade, CardType.Jack),
                                new Card(CardSuit.Spade, CardType.Queen),
                                new Card(CardSuit.Spade, CardType.King)
                            },
                        HandRankType.StraightFlush,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Ten
                            }
                    },
                new object[]
                    {
                        new[]
                            {
                                new Card(CardSuit.Spade, CardType.Ten),
                                new Card(CardSuit.Spade, CardType.Jack),
                                new Card(CardSuit.Spade, CardType.Queen),
                                new Card(CardSuit.Spade, CardType.King),
                                new Card(CardSuit.Spade, CardType.King),
                                new Card(CardSuit.Spade, CardType.Ace),
                                new Card(CardSuit.Spade, CardType.Ace)
                            },
                        HandRankType.StraightFlush,
                        new[]
                            {
                                CardType.Ace, CardType.King,
                                CardType.Queen, CardType.Jack,
                                CardType.Ten
                            }
                    }
            };

        [Test]
        [TestCaseSource(nameof(HighCardCases))]
        [TestCaseSource(nameof(PairCases))]
        [TestCaseSource(nameof(TwoPairsCases))]
        [TestCaseSource(nameof(ThreeOfAKindCases))]
        [TestCaseSource(nameof(StraightCases))]
        [TestCaseSource(nameof(FlushCases))]
        [TestCaseSource(nameof(FullHouseCases))]
        [TestCaseSource(nameof(FourOfAKindCases))]
        [TestCaseSource(nameof(StraightFlushCases))]
        public void GetRankTypeShouldWorkCorrectly(ICollection<Card> playerCards, HandRankType expectedHandRankType, ICollection<CardType> expectedBestHandCards)
        {
            IHandEvaluator handEvaluator = new HandEvaluator();
            var bestHand = handEvaluator.GetBestHand(playerCards.Shuffle().ToList());
            Assert.AreEqual(expectedHandRankType, bestHand.RankType);
            CollectionAssert.AreEquivalent(expectedBestHandCards, bestHand.Cards);
        }
    }
}

namespace TexasHoldem.Tests.Helpers
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using TexasHoldem.Logic.Helpers;
    using TexasHoldem.Logic.Players;

    [TestFixture]
    public class ActionValidatorTests
    {
        private const string FirstPlayer = "FirstPlayer";

        private const string SecondPlayer = "SecondPlayer";

        private const string ThirdPlayer = "ThirdPlayer";

        // TODO: Add more tests
        private static readonly object[] IsValidCases =
            {
                new object[]
                    {
                        true,
                        new List<PlayerActionAndName>
                            {
                                new PlayerActionAndName(FirstPlayer, PlayerAction.Fold())
                            },
                        new PlayerActionAndName(SecondPlayer, PlayerAction.Fold()),
                        10000
                    },
                new object[]
                    {
                        true,
                        new List<PlayerActionAndName>
                            {
                                new PlayerActionAndName(FirstPlayer, PlayerAction.Check())
                            },
                        new PlayerActionAndName(SecondPlayer, PlayerAction.Check()),
                        10000
                    },
                new object[]
                    {
                        true,
                        new List<PlayerActionAndName>
                            {
                                new PlayerActionAndName(FirstPlayer, PlayerAction.Check())
                            },
                        new PlayerActionAndName(SecondPlayer, PlayerAction.Raise(10)),
                        10000
                    },
                new object[]
                    {
                        true,
                        new List<PlayerActionAndName>
                            {
                                new PlayerActionAndName(FirstPlayer, PlayerAction.Check())
                            },
                        new PlayerActionAndName(SecondPlayer, PlayerAction.Raise(100)),
                        10
                    },
                new object[]
                    {
                        false,
                        new List<PlayerActionAndName>
                            {
                                new PlayerActionAndName(FirstPlayer, PlayerAction.Raise(1))
                            },
                        new PlayerActionAndName(SecondPlayer, PlayerAction.Check()),
                        10000
                    },
                new object[]
                    {
                        true,
                        new List<PlayerActionAndName>
                            {
                                new PlayerActionAndName(FirstPlayer, PlayerAction.Raise(1))
                            },
                        new PlayerActionAndName(SecondPlayer, PlayerAction.Call()),
                        10000
                    },
                new object[]
                    {
                        true,
                        new List<PlayerActionAndName>
                            {
                                new PlayerActionAndName(FirstPlayer, PlayerAction.Raise(1))
                            },
                        new PlayerActionAndName(SecondPlayer, PlayerAction.Raise(1)),
                        10000
                    },
                new object[]
                    {
                        false,
                        new List<PlayerActionAndName>
                            {
                                new PlayerActionAndName(FirstPlayer, PlayerAction.Raise(10))
                            },
                        new PlayerActionAndName(SecondPlayer, PlayerAction.Raise(9)),
                        10000
                    },
            };

        [Test]
        [TestCaseSource(nameof(IsValidCases))]
        public void IsValidShouldReturnCorrectValues(bool expectedResult, ICollection<PlayerActionAndName> previousActions, PlayerActionAndName currentAction, int playerMoney)
        {
            IActionValidator actionValidator = new ActionValidator();
            var actualResult = actionValidator.IsValid(previousActions, currentAction, playerMoney);
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}

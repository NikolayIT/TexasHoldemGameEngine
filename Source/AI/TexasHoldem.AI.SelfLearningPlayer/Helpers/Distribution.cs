namespace TexasHoldem.AI.SelfLearningPlayer.Helpers
{
    using System;

    public class Distribution
    {
        /// <summary>
        /// Returns a quadratic dependence between the position of the hero and his action
        /// </summary>
        /// <param name="heroPosition">The position of the hero [0..TotalPlayers-1]</param>
        /// <param name="numberOfOpponents">Number of opponents [1..TotalPlayers-1]</param>
        /// <param name="range">The range that is quadratically distributed over the positions</param>
        /// <param name="slope">Slope of a parabola</param>
        /// <returns>The value is from zero to one</returns>
        public static double FrequencyOfActionFromASpecificPosition(
            int heroPosition,
            int numberOfOpponents,
            double range,
            double slope)
        {
            if (heroPosition > numberOfOpponents)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(heroPosition), "The position of the hero must be from 0 to the total number of players");
            }

            var coefficient = -(range * (Math.Pow(2, slope) - 1.0))
                / (Math.Pow(2, slope) - Math.Pow(2, (numberOfOpponents * slope) + (2.0 * slope)));
            return Math.Pow(2.0, slope * (heroPosition + 1)) * coefficient;
        }
    }
}

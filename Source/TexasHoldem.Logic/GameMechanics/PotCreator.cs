namespace TexasHoldem.Logic.GameMechanics
{
    using System.Collections.Generic;
    using System.Linq;

    internal class PotCreator
    {
        private IList<InternalPlayer> players;

        public PotCreator(IList<InternalPlayer> players)
        {
            this.players = players;
        }

        public Pot MainPot
        {
            get
            {
                var levels = this.Levels();
                var upperLimit = levels.First();
                return this.Create(0, upperLimit);
            }
        }

        public List<Pot> SidePots
        {
            get
            {
                var pots = new List<Pot>();
                var levels = this.Levels();

                if (levels.Count > 1)
                {
                    var list = levels.ToList();

                    for (int i = 0; i < list.Count - 1; i++)
                    {
                        var pot = this.Create(list[i], list[i + 1]);

                        if (pot.AmountOfMoney != 0)
                        {
                            pots.Add(pot);
                        }
                    }
                }

                return pots;
            }
        }

        private SortedSet<int> Levels()
        {
            var levels = new SortedSet<int> { int.MaxValue };

            foreach (var item in this.players)
            {
                if (item.PlayerMoney.Money <= 0)
                {
                    levels.Add(item.PlayerMoney.CurrentlyInPot);
                }
            }

            return levels;
        }

        private Pot Create(int lowerLimit, int upperLimit)
        {
            var amountOfMoney = 0;
            var participants = new List<string>();

            foreach (var item in this.players)
            {
                if (item.PlayerMoney.CurrentlyInPot > lowerLimit && item.PlayerMoney.CurrentlyInPot <= upperLimit)
                {
                    amountOfMoney += item.PlayerMoney.CurrentlyInPot - lowerLimit;
                    participants.Add(item.Name);
                }
                else if (item.PlayerMoney.CurrentlyInPot > upperLimit)
                {
                    amountOfMoney += upperLimit - lowerLimit;
                    participants.Add(item.Name);
                }
            }

            return new Pot(amountOfMoney, participants);
        }
    }
}

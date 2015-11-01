namespace TexasHoldem.AI.SmartPlayer
{
    using System;

    using TexasHoldem.AI.SmartPlayer.Helpers;
    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Players;

    public class SmartPlayer : BasePlayer
    {
        public override string Name { get; } = "SmartPlayer_" + Guid.NewGuid();

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            if (context.RoundType == GameRoundType.PreFlop)
            {
                var playHand = HandStrengthValuation.PreFlop(this.FirstCard, this.SecondCard);
                if (playHand == CardValuationType.Unplayable)
                {
                    return PlayerAction.Fold();
                }
                else if (playHand == CardValuationType.Recommended)
                {
                    return PlayerAction.Raise(context.SmallBlind * 2);
                }
                else
                {
                    return PlayerAction.CheckOrCall();
                }
            }

            return PlayerAction.CheckOrCall();
        }
    }
}

namespace TexasHoldem.AI.SmartPlayer
{
    using TexasHoldem.Logic.Players;

    public class SmartPlayer : BasePlayer
    {
        public override string Name => "Smart player";

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            // TODO: Implement smart logic
            return PlayerAction.Fold();
        }
    }
}

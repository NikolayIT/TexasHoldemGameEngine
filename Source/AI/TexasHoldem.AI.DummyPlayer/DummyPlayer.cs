namespace TexasHoldem.AI.DummyPlayer
{
    using TexasHoldem.Logic.Players;

    public class DummyPlayer : BasePlayer
    {
        public override string Name => "Dummy player";

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            // TODO: Raise/Call(Check)/Fold on random
            return PlayerAction.Fold();
        }
    }
}

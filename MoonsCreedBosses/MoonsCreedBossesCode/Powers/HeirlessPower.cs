using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace MoonsCreedBosses.MoonsCreedBossesCode.Powers;

public class HeirlessPower : MoonsCreedBossesPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card.IsUpgraded && cardPlay.Card.Owner.Creature == Owner)
        {
            var c = cardPlay.Card;
            CreatureCmd.Damage(choiceContext, c.Owner.Creature, Amount, ValueProp.Unpowered, null, null);
        }
        return base.AfterCardPlayed(choiceContext, cardPlay);
    }
}
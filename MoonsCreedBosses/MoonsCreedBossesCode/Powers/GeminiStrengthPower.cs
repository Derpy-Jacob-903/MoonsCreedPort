using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MoonsCreedBosses.MoonsCreedBossesCode.Powers;

public class GeminiStrengthPower : MoonsCreedBossesPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override int DisplayAmount => CombatState.Creatures.Where(c => c.Side == Owner.Side)
        .Sum(creature => Math.Abs(creature.CurrentHp - Owner.CurrentHp));

    protected override object InitInternalData() => new VigorPower.Data();

    public override Decimal ModifyDamageAdditive(
        Creature target,
        Decimal amount,
        ValueProp props,
        Creature dealer,
        CardModel cardSource,
        CardPlay cardPlay)
    {
        var __amount = CombatState.Creatures.Where(c => c.Side == Owner.Side).Sum(creature => Math.Abs(creature.CurrentHp - Owner.CurrentHp));
        return Owner != dealer || !props.IsPoweredAttack() ? 0M : __amount ;
    }
}
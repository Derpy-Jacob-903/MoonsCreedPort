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

public class GeminiDexterityPower : MoonsCreedBossesPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public override int DisplayAmount => CombatState.Creatures.Where(c => c.Side == Owner.Side)
        .Sum(creature => Math.Abs(creature.CurrentHp - Owner.CurrentHp));

    public override decimal ModifyBlockAdditive(Creature target, decimal block, ValueProp props, CardModel cardSource, CardPlay cardPlay)
    {
        var __amount = CombatState.Creatures.Where(c => c.Side == Owner.Side).Sum(creature => Math.Abs(creature.CurrentHp - Owner.CurrentHp));
        return Owner != target || props.HasFlag(ValueProp.Move) ? 0M : __amount ;
    }
}
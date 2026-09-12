using BaseLib.Abstracts;
using BaseLib.Hooks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MoonsCreedBosses.MoonsCreedBossesCode.Powers;

public class GraspPower : MoonsCreedBossesPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override Task AfterCurrentHpChanged(Creature creature, decimal delta)
    {
        if (delta <= 0)
        {
            return base.AfterCurrentHpChanged(creature, delta);
        }
        if (Owner.IsPlayer || Owner.IsPet)
        {
            if (creature.IsMonster)
            {
                CreatureCmd.GainMaxHp(Owner, Amount);
            }
        }
        else
        {
            if (creature.IsPlayer || creature.IsPet)
            {
                CreatureCmd.GainMaxHp(Owner, Amount);
            }
        }
        return base.AfterCurrentHpChanged(creature, delta);
    }
}
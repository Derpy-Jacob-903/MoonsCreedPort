using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
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

public class FriendlyAquariusDebuffPower : MoonsCreedBossesPower
{
    public override Decimal ModifyDamageMultiplicative(
        Creature target,
        Decimal amount,
        ValueProp props,
        Creature dealer,
        CardModel cardSource,
        CardPlay cardPlay)
    {
        if (dealer != Owner || !props.IsPoweredAttack())
            return 1M;
        return DynamicVars["DamageDecrease"].BaseValue;
    }
    
    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != CombatSide.Enemy)
            return;
        await PowerCmd.TickDownDuration(this);
    }
    
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
}
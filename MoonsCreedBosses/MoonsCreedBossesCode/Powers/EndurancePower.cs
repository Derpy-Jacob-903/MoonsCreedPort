using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MoonsCreedBosses.MoonsCreedBossesCode.Powers;

public class EndurancePower : MoonsCreedBossesPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task BeforeSideTurnEndEarly(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (!participants.Contains(Owner))
            return;
        Flash();
        Decimal num = await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
    }

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props,
        Creature dealer, CardModel cardSource)
    {
        if (target != Owner || !props.IsPoweredAttack()) return;
        Flash();
        Decimal num = await CreatureCmd.GainBlock(Owner, Amount, ValueProp.Unpowered, null);
    }

    public override bool ShouldScaleInMultiplayer => true;

    public override Decimal GetScaledAmountForMultiplayer(
        ICombatState combatState,
        Creature applier,
        Decimal amount,
        Creature target,
        CardModel cardSource)
    {
        return combatState.Players.Count * amount;
    }
}
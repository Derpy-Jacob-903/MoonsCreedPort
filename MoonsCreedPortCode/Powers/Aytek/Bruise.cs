using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MoonsCreedPort.MoonsCreedPortCode.Powers.Aytek;

public class BruisePower : MoonsCreedPortPower
{
    public override decimal ModifyDamageAdditive(Creature target, decimal amount, ValueProp props, Creature dealer,
        CardModel cardSource, CardPlay cardPlay)
        => target != Owner || !props.HasFlag(ValueProp.Move) || props.HasFlag(ValueProp.Unpowered) ? 0 : Amount;

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != Owner.Side)
            return;
        await PowerCmd.Remove(this);
    }

    protected override IEnumerable<DynamicVar> CanonicalVars => [new DynamicVar("CardsLeft", 0M)];

    public override Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props,
        Creature dealer, CardModel cardSource)
    {
        if (target == Owner && !props.HasFlag(ValueProp.Unpowered) && props.HasFlag(ValueProp.Move))
        {
            ++DynamicVars["CardsLeft"].BaseValue;
        }
        return base.AfterDamageReceived(choiceContext, target, result, props, dealer, cardSource);
    }

    public override Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        if (Amount - DynamicVars["CardsLeft"].BaseValue > 0)
        {
            PowerCmd.ModifyAmount(choiceContext, this, -DynamicVars["CardsLeft"].BaseValue, null, null);
            DynamicVars["CardsLeft"].BaseValue = 0;
        }
        else
        {
            DynamicVars["CardsLeft"].BaseValue = 0;
            PowerCmd.Remove(this);
        }
        return base.AfterAttack(choiceContext, command);
    }

    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
}
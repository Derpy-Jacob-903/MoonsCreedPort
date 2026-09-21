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

public class CancerVigorPower : MoonsCreedBossesPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    protected override object InitInternalData() => new VigorPower.Data();

    public override Task BeforeAttack(AttackCommand command)
    {
        if (command.Attacker != Owner || !command.DamageProps.IsPoweredAttack())
            return Task.CompletedTask;
        VigorPower.Data internalData = GetInternalData<VigorPower.Data>();
        if (internalData.commandToModify != null || command.ModelSource != null && command.ModelSource is not CardModel || !command.DamageProps.IsPoweredAttack())
            return Task.CompletedTask;
        internalData.commandToModify = command;
        internalData.amountWhenAttackStarted = Amount;
        return Task.CompletedTask;
    }

    public override Decimal ModifyDamageAdditive(
        Creature target,
        Decimal amount,
        ValueProp props,
        Creature dealer,
        CardModel cardSource,
        CardPlay cardPlay)
    {
        if (this.Owner != dealer || !props.IsPoweredAttack())
            return 0M;
        VigorPower.Data internalData = GetInternalData<VigorPower.Data>();
        return internalData.commandToModify != null && cardSource != null && cardSource != internalData.commandToModify.ModelSource || internalData.commandToModify != null && internalData.commandToModify.Attacker != dealer ? 0M : ((this.Amount / 100m) * Owner.MaxHp  );
    }

    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        VigorPower.Data internalData = GetInternalData<VigorPower.Data>();
        if (command != internalData.commandToModify)
            return;
        await PowerCmd.ModifyAmount(choiceContext, this, -internalData.amountWhenAttackStarted, null, null);
    }
}
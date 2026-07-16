using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MoonsCreedPort.MoonsCreedPortCode.Powers;

public class ArcraneChargePower : MoonsCreedPortPower
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override object InitInternalData() => new ArcraneChargePower.Data();

    public override Task BeforeAttack(AttackCommand command)
    {
        if (command.Attacker != this.Owner || !command.DamageProps.IsPoweredAttack())
            return Task.CompletedTask;
        Data internalData = this.GetInternalData<Data>();
        if (internalData.commandToModify != null || command.ModelSource != null && !(command.ModelSource is CardModel cardModel && cardModel.Keywords.Contains(AytekStuff.ArcraneCast)) || !command.DamageProps.IsPoweredAttack())
            return Task.CompletedTask;
        internalData.commandToModify = command;
        internalData.amountWhenAttackStarted = this.Amount;
        return Task.CompletedTask;
    }

    public override Decimal ModifyDamageAdditive(
        Creature target,
        decimal amount,
        ValueProp props,
        Creature dealer,
        CardModel cardSource, CardPlay cardPlay)
    {
        if (this.Owner != dealer || !props.IsPoweredAttack() || cardSource == null || !cardSource.Keywords.Contains(AytekStuff.ArcraneCast))
            return 0M;
        Data internalData = this.GetInternalData<Data>();
        return internalData.commandToModify != null && cardSource != internalData.commandToModify.ModelSource || internalData.commandToModify != null && internalData.commandToModify.Attacker != dealer ? 0M : (Decimal) this.Amount;
    }

    public override async Task AfterAttack(PlayerChoiceContext choiceContext, AttackCommand command)
    {
        Data internalData = GetInternalData<Data>();
        if (command != internalData.commandToModify)
            return;
        var num = await PowerCmd.ModifyAmount(choiceContext, this, -internalData.amountWhenAttackStarted, null, null);
    }

    private class Data
    {
        public AttackCommand? commandToModify;
        public int amountWhenAttackStarted;
    }
}
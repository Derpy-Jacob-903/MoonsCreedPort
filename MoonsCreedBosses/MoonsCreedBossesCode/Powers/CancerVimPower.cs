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

public class CancerVimPower : MoonsCreedBossesPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    protected override object InitInternalData() => new Data();

    public override Task BeforeBlockGained(Creature creature, decimal amount, ValueProp props, CardModel cardSource)
    {
        if (creature != Owner || !props.IsPoweredCardOrMonsterMoveBlock())
            return Task.CompletedTask;
        Data internalData = GetInternalData<Data>();
        internalData.amountWhenAttackStarted = Amount;
        return Task.CompletedTask;
    }

    public override decimal ModifyBlockMultiplicative(Creature target, decimal block, ValueProp props, CardModel cardSource,
        CardPlay cardPlay)
    {
        if (this.Owner != target || !props.IsPoweredCardOrMonsterMoveBlock())
            return 0M;
        return Math.Floor((Amount / 100m) * Owner.MaxHp);
    }

    public override async Task AfterBlockGained(Creature creature, decimal amount, ValueProp props, CardModel cardSource)
    {
        Data internalData = GetInternalData<Data>();
        await PowerCmd.ModifyAmount(new ThrowingPlayerChoiceContext(), this, -internalData.amountWhenAttackStarted, null, null);
    }
    
    public class Data
    {
        public int amountWhenAttackStarted;
    }
}
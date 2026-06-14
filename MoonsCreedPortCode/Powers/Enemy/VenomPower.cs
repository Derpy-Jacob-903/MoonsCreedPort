using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace MoonsCreedPort.MoonsCreedPortCode.Powers.Enemy;

public class VenomPower : MoonsCreedPortPower
{
    public override PowerType Type => PowerType.Debuff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override Color AmountLabelColor => PowerModel._normalAmountLabelColor;

    private int TriggerCount => 1;
    //return Math.Min(this.Amount, 1 + this.Owner.CombatState.GetOpponentsOf(this.Owner).Where<Creature>((Func<Creature, bool>) (c => c.IsAlive)).Sum<Creature>((Func<Creature, int>) (a => a.GetPowerAmount<AccelerantPower>())));

    public int CalculateTotalDamageNextTurn()
    {
        Decimal totalDamageNextTurn = 0M;
        int num1 = Math.Min(this.Amount, this.TriggerCount);
        for (int index = 0; index < num1; ++index)
        {
            Decimal num2 = Hook.ModifyDamage(this.Owner.CombatState.RunState, this.Owner.CombatState, this.Owner, (Creature) null, (Decimal) (this.Amount - index), ValueProp.Unblockable | ValueProp.Unpowered, null, ModifyDamageHookType.All, CardPreviewMode.None, out IEnumerable<AbstractModel> _);
            totalDamageNextTurn += num2;
        }
        return (int) totalDamageNextTurn;
    }
    
    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != Owner.Side)
            return;
        int iterations = this.TriggerCount;
        for (int i = 0; i < iterations; ++i)
        {
            IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage((PlayerChoiceContext) new ThrowingPlayerChoiceContext(), Owner, Amount, ValueProp.Unblockable | ValueProp.Unpowered, null, null);
            if (Owner.IsAlive)
                await PowerCmd.Decrement(this);
            else
                await Cmd.CustomScaledWait(0.1f, 0.25f);
        }
    }
}
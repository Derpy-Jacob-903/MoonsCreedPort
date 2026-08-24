using BaseLib.Cards.Variables;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

namespace MoonsCreedPort.MoonsCreedPortCode.Powers.Aytek;

public class TechDominancePower : MoonsCreedPortPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CalculationBaseVar(0M),
        new CalculationExtraVar(5M),
        /*new CalculatedVar("CalculatedHits").WithMultiplier((Func<CardModel, Creature, Decimal>)((card, _) =>
        {
            if (Owner.Player?.PlayerCombatState != null)
            {
                return Owner.Player.PlayerCombatState.Stars * 5;
            }
            return 0;
        }))*/
    ];

    public override decimal ModifyDamageMultiplicative(
        Creature target,
        decimal amount,
        ValueProp props,
        Creature dealer,
        CardModel cardSource,
        CardPlay cardPlay)
    {
        if (!props.IsPoweredAttack() || cardSource == null || cardSource.Owner.Creature != this.Owner)
            return 1M;
        return 1M + (Decimal) this.Amount * Owner.Player.PlayerCombatState.Stars / 100M;
    }
}
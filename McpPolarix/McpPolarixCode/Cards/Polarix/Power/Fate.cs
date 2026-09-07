using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;

namespace McpPolarix.McpPolarixCode.Cards;

public class Fate() : PolarixCard(0,
    CardType.Power, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<StrengthPower>(1)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (CombatState != null)
        {
            await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
            await PowerCmd.Apply<StrengthPower>(context, Owner.Creature,
                CombatState.HittableEnemies.Count * DynamicVars.Strength.BaseValue, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Strength.UpgradeValueBy(1M);
}
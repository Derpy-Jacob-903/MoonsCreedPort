using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace McpPolarix.McpPolarixCode.Cards;

public class SoulSiphon() : PolarixCard(2,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(5m, ValueProp.Move),
        new HealVar(4)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (CombatState != null)
        {
            var damageResults = await CreatureCmd.Damage(context, CombatState.HittableEnemies, DynamicVars.Damage, play.Card.Owner.Creature);
            await CreatureCmd.Heal(Owner.Creature,
                damageResults.Count<DamageResult>((Func<DamageResult, bool>)(r => r.TotalDamage > 0)));
        }
    }
    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
}
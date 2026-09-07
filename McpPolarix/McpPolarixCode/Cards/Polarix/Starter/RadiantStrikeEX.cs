using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace McpPolarix.McpPolarixCode.Cards;

public class RadiantStrikeEx() : PolarixCard(2,
    CardType.Attack, CardRarity.Ancient,
    TargetType.AnyEnemy)
{
    
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Strike };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(20m, ValueProp.Move),
        new PowerVar<WeakPower>(5M)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (play.Target != null)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .FromCard(play.Card, play).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(context);
            await PowerCmd.Apply<WeakPower>(context, play.Target, DynamicVars.Weak.BaseValue, Owner.Creature, this);
        }
    }
    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(10M);
}
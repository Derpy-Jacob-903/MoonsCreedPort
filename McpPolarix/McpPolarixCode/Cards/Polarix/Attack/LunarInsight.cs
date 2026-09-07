using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace McpPolarix.McpPolarixCode.Cards;

public class LunarInsight() : PolarixCard(1,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(3m, ValueProp.Move),
        new RepeatVar(2),
        new CardsVar(1)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (CombatState != null)
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .FromCard(play.Card, play).TargetingRandomOpponents(CombatState)
                .WithHitCount(DynamicVars.Repeat.IntValue)
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(context);
    }
    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
}
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Powers;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class SeekerMissile() : AytekCard(1,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override HashSet<CardTag> CanonicalTags => [AytekStuff.Missile];
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        ..MakeCalculatedDamage(5, (_, c) => c != null && c.HasPower<VulnerablePower>() ? 1 : 0, 10)
        //new DamageVar(5m, ValueProp.Move),
        //new DamageVar("VulnDamage", 15m, ValueProp.Move),
        //new ExtraDamageVar(10)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (play.Target != null)
            //await DamageCmd.Attack(play.Target.HasPower<VulnerablePower>() ? DynamicVars["VulnDamage"].BaseValue : DynamicVars.Damage.BaseValue)
            await DamageCmd.Attack(DynamicVars.CalculatedDamage.Calculate(play.Target))
                .FromCard(play.Card, play).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(context);
    }
    protected override void OnUpgrade()
    {
        this.DynamicVars.CalculationBase.UpgradeValueBy(3M);
        this.DynamicVars.ExtraDamage.UpgradeValueBy(6M);
    } 
}
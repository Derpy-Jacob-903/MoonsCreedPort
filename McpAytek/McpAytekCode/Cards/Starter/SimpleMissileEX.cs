using BaseLib.Cards.Variables;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Powers;

namespace McpAytek.McpAytekCode.Cards;

public class SimpleMissileEx() : AytekCard(1,
    CardType.Attack, CardRarity.Ancient,
    TargetType.AnyEnemy), ITechKeyword
{
    public override int CanonicalStarCost => 1;
    protected override HashSet<CardTag> CanonicalTags => [AytekStuff.Missile];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        ..MakeTechDamage(this, 15, 10),
        new PowerVar<VulnerablePower>(2)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (CombatState != null)
        {
            await DamageCmd.Attack(DynamicVars.CalculatedDamage)
                .FromCard(play.Card, play).TargetingAllOpponents(CombatState)
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(context);
            //if (TriggeredTech(play))
                //await PowerCmd.Apply<VulnerablePower>(context, CombatState.HittableEnemies, DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);
        }
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.CalculationBase.UpgradeValueBy(7M);
        this.DynamicVars.Vulnerable.UpgradeValueBy(1M);
    }
}
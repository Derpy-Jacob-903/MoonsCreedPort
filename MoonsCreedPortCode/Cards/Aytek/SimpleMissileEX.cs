using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Powers;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class SimpleMissileEx() : AytekCard(1,
    CardType.Attack, CardRarity.Ancient,
    TargetType.AnyEnemy), ITechKeyword
{
    public override int CanonicalStarCost => 1;
    protected override HashSet<CardTag> CanonicalTags => [AytekStuff.Missile];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(8M,ValueProp.Move), 
        new DamageVar("TechDamage", 12M, ValueProp.Move), 
        new DynamicVar("TechExtra", 4M),
        //new CalculatedDamageVar(ValueProp.Move).WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => this.WillTriggerTech ? 1 : 0))
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (play.Target != null)
        {
            await DamageCmd.Attack(TechATK(play))
                .FromCard(this).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(context);
        }
    }
    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
}
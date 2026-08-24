using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Arcrane;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Powers;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Arcrane;

public class MagicMissileEx() : ArcraneCard(2,
    CardType.Attack, CardRarity.Ancient,
    TargetType.AnyEnemy)
{
    protected override HashSet<CardTag> CanonicalTags => [AytekStuff.Missile];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [ CardKeyword.Retain ];
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        ..MakeCalculatedDamage(25, (Func<CardModel, Creature, Decimal>) ((card, _) => card.Owner.Creature.GetPowerAmount<ArcraneChargePower>())),
        new ChargeVar(10)
        //new CalculatedDamageVar(ValueProp.Move).WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => this.WillTriggerTech ? 1 : 0))
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (play.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.CalculatedDamage)
                .FromCard(play.Card, play).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(context);
            
        }
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
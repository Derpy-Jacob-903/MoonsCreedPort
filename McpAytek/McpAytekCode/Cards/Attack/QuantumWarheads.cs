using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Powers;

namespace McpAytek.McpAytekCode.Cards;

public class QuantumWarheads() : AytekCard(1,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override HashSet<CardTag> CanonicalTags => [AytekStuff.Missile];
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CalculationBaseVar(8),
        new ExtraDamageVar(2),
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((Func<CardModel, Creature, decimal>) ((card, _) 
                => card.Owner.PlayerCombatState!.Stars
            ))
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (play.Target != null)
            await DamageCmd.Attack(base.DynamicVars.CalculatedDamage.Calculate(play.Target))
                .FromCard(play.Card, play).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(context);
    }
    protected override void OnUpgrade() => this.DynamicVars.ExtraDamage.UpgradeValueBy(1M);
}
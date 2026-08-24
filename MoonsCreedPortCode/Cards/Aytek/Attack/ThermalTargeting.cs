using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Powers.Aytek;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class ThermalTargeting() : AytekCard(1,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy), ITechKeyword
{
    public override int CanonicalStarCost => 1;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(1m, ValueProp.Move),
        new RepeatVar(4),
        new PowerVar<BruisePower>(2)
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        AytekStuff.GunKeyword
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        IReadOnlyList<Creature> hittableEnemies = CombatState.HittableEnemies;
        if (hittableEnemies.Count == 0)
            return;
        Creature weakestEnemy = hittableEnemies.MinBy((Func<Creature, int>) (c => c.CurrentHp));
        if (weakestEnemy != null)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .WithHitCount(DynamicVars.Repeat.IntValue)
                .FromCard(play.Card, play).Targeting(weakestEnemy)
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(context);
        }
        if (!TriggeredTech(play) || hittableEnemies.Count == 0) return;
        weakestEnemy = hittableEnemies.MinBy((Func<Creature, int>) (c => c.CurrentHp));
        if (weakestEnemy != null) await CommonActions.Apply<BruisePower>(context, weakestEnemy, this);
    }
    protected override void OnUpgrade() => this.DynamicVars.Repeat.UpgradeValueBy(2M);
}
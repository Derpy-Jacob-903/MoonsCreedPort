using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Powers;

namespace McpAytek.McpAytekCode.Cards;

public class KillProcess() : AytekCard(3,
    CardType.Attack, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    public override bool HasStarCostX => true;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(9m, ValueProp.Move),
        //..MakeCalculatedVar("isRaw", 1, Bonus)
    ];

    private int Bonus(Creature target)
    {
        return (CombatManager.Instance?.History.CardPlaysFinished.Count((Func<CardPlayFinishedEntry, bool>)(e => e.HappenedThisTurn(this.CombatState) && e.CardPlay.Target == target)) < 1
            ? 2
            : 1);
    }

    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        //List<Creature> validTargets = AytekStuff.GetPossibleTargets().Where<Creature>((Func<Creature, bool>) (c => c.IsAlive)).ToList<Creature>();
        //play.Target = Owner.RunState.Rng.CombatTargets.NextItem<Creature>((IEnumerable<Creature>) validTargets);
        if (play.Target != null)
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(play.Card, play).Targeting(play.Target).WithHitCount((int)(ResolveStarXValue() * Bonus(play.Target)))
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(context);
    }

    protected override bool ShouldGlowGoldInternal => CombatState != null && CombatState.HittableEnemies.Any(c => Bonus(c) == 2);

    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(5M);
}
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Powers;

namespace McpAytek.McpAytekCode.Cards;

public class AytekGun() : AytekCard(0,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(3m, ValueProp.Move),
        new RepeatVar(3)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [
            AytekStuff.GunKeyword
        ];

    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        //List<Creature> validTargets = AytekStuff.GetPossibleTargets().Where<Creature>((Func<Creature, bool>) (c => c.IsAlive)).ToList<Creature>();
        //play.Target = Owner.RunState.Rng.CombatTargets.NextItem<Creature>((IEnumerable<Creature>) validTargets);
        if (play.Target != null)
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount(DynamicVars.Repeat.IntValue)
                .FromCard(play.Card, play).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(context);
    }
    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(1M);
}
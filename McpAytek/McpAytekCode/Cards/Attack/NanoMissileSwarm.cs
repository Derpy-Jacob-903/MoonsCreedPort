using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Powers;

namespace McpAytek.McpAytekCode.Cards;

public class NanoMissileSwarm() : AytekCard(1,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override HashSet<CardTag> CanonicalTags => [AytekStuff.Missile];
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(5M, ValueProp.Move),
        new CalculationBaseVar(0M),
        new CalculationExtraVar(1M),
        new CalculatedVar("CalculatedHits").WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => (Decimal) PileType.Hand.GetPile(card.Owner).Cards.Count<CardModel>((Func<CardModel, bool>) (c => c.Tags.Contains(AytekStuff.Missile)))))
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (play.Target != null)
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount((int) ((CalculatedVar) DynamicVars["CalculatedHits"]).Calculate(play.Target) + 1)
                .FromCard(play.Card, play).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash", tmpSfx: "dagger_throw.mp3").Execute(context);
    }
    protected override bool ShouldGlowRedInternal => ((CalculatedVar)DynamicVars["CalculatedHits"]).Calculate(null) == 0;
    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(2M);
}
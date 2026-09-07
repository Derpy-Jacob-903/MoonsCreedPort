using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Powers;

namespace McpAytek.McpAytekCode.Cards;

public class SimpleMissile() : AytekCard(1,
    CardType.Attack, CardRarity.Basic,
    TargetType.AnyEnemy), ITranscendenceCard, ITechKeyword
{
    public override int CanonicalStarCost => 1;
    protected override HashSet<CardTag> CanonicalTags => [AytekStuff.Missile];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        ..MakeTechDamage(this, 8, 4)
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
    protected override void OnUpgrade() => this.DynamicVars.CalculationBase.UpgradeValueBy(3M);
    public CardModel GetTranscendenceTransformedCard()
    {
        return ModelDb.Card<SimpleMissileEx>();
    }
}
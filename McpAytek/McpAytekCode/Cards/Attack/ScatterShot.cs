using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

namespace McpAytek.McpAytekCode.Cards;

public class ScatterShot() : AytekCard(1,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy), ITechKeyword
{
    public override int CanonicalStarCost => 1;
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(2m, ValueProp.Move),
        new RepeatVar(3),
        new PowerVar<VulnerablePower>(2)
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        AytekStuff.GunKeyword
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (play.Target != null)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .WithHitCount(DynamicVars.Repeat.IntValue)
                .FromCard(play.Card, play).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(context);
            if (!TriggeredTech(play) || CombatState == null) return;
            await CommonActions.Apply<VulnerablePower>(context, play.Target, this);
        }
    }
    protected override void OnUpgrade() => this.DynamicVars.Vulnerable.UpgradeValueBy(1M);
}
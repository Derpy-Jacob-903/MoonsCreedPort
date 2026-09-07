using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

namespace McpAytek.McpAytekCode.Cards;

public class TacticalStrike() : AytekCard(2,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy), ITechKeyword
{
    public override int CanonicalStarCost => 1;
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(2m, ValueProp.Move),
        new RepeatVar(7),
        new CardsVar(1)
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
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .WithHitCount(DynamicVars.Repeat.IntValue)
                .FromCard(play.Card, play).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(context);
        if (!TriggeredTech(play) || Owner == null) return;
        await CommonActions.Draw(this, context);
    }
    protected override void OnUpgrade()
    {
        this.DynamicVars.Repeat.UpgradeValueBy(2M);
        this.DynamicVars.Cards.UpgradeValueBy(1M);
    }
}
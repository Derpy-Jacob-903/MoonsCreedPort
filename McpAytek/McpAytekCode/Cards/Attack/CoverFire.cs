using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

namespace McpAytek.McpAytekCode.Cards;

public class CoverFire() : AytekCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy), ITechKeyword
{
    public override int CanonicalStarCost => 1;
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(2m, ValueProp.Move),
        new RepeatVar(3),
        new BlockVar(5, ValueProp.Move)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (CombatState != null)
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount(DynamicVars.Repeat.IntValue)
                .FromCard(play.Card, play).TargetingRandomOpponents(CombatState, true)
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(context);
        if (!TriggeredTech(play) || CombatState == null) return;
        await CommonActions.CardBlock(this, DynamicVars.Block, play);

    }
    protected override void OnUpgrade()
    {
        this.DynamicVars.Repeat.UpgradeValueBy(1M);
        DynamicVars.Block.UpgradeValueBy(3);
    }
}
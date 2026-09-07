using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

namespace McpAytek.McpAytekCode.Cards;

public class ZeroPointKick() : AytekCard(2,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(18m, ValueProp.Move),
        new EnergyVar(1)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (play.Target != null)
        {
            if (!(await DamageCmd.Attack(DynamicVars.Damage.BaseValue).FromCard(play.Card, play).Targeting(play.Target).WithHitFx("vfx/vfx_attack_slash", tmpSfx: "slash_attack.mp3").Execute(context)).Results.SelectMany<List<DamageResult>, DamageResult>((Func<List<DamageResult>, IEnumerable<DamageResult>>) (r => (IEnumerable<DamageResult>) r)).Any<DamageResult>((Func<DamageResult, bool>) (r => r.WasTargetKilled)))
                return;
            await PlayerCmd.GainEnergy(DynamicVars.Energy.IntValue, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Damage.UpgradeValueBy(6M);
        this.DynamicVars.Energy.UpgradeValueBy(1M);
    }
}
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Character.Echo;
using MoonsCreedPort.MoonsCreedPortCode.Character.Polarix;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Polarix;

public class Misery() : PolarixCard(0,
    CardType.Skill, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<VulnerablePower>(1m)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (play.Target != null)
            await PowerCmd.Apply<VulnerablePower>(context, play.Target, DynamicVars.Vulnerable.BaseValue,
                Owner.Creature, this);
    }

    protected override void OnUpgrade() => this.DynamicVars.Vulnerable.UpgradeValueBy(1M);
}
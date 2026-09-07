using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MoonsCreedPort.MoonsCreedPortCode.Cards.Polarix;

namespace McpPolarix.McpPolarixCode.Cards;

public class ForbiddenPower() : PolarixCard(0,
    CardType.Skill, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(1),
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [ HoverTipFactory.FromCard<Exhaustion>() ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        Log.Warn(this.Id.Entry + ": This card is unimplemented!!");
        await CreatureCmd.GainBlock(Owner.Creature, base.DynamicVars.Block, play);
    }

    protected override void OnUpgrade() => this.DynamicVars.Energy.UpgradeValueBy(1M);
}
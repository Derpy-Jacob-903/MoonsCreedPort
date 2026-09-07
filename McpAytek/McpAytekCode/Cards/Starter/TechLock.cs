using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

namespace McpAytek.McpAytekCode.Cards;

public class TechLock() : AytekCard(0,
    CardType.Skill, CardRarity.Basic,
    TargetType.Self), ITechKeyword
{
    public override int CanonicalStarCost => 1;
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        ..MakeTechBlock(this, 3, 3)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, new BlockVar(DynamicVars.CalculatedBlock.Calculate(null), DynamicVars.CalculatedBlock.Props), play);
    }
    protected override void OnUpgrade() => this.DynamicVars.CalculationBase.UpgradeValueBy(3M);
}
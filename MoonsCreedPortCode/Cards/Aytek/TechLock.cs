using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class TechLock() : AytekCard(0,
    CardType.Skill, CardRarity.Basic,
    TargetType.Self), ITechKeyword
{
    public override int CanonicalStarCost => 1;
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(3M, ValueProp.Move), 
        new BlockVar("TechBlock", 6M, ValueProp.Move), 
        new DynamicVar("TechExtra", 3M),
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, TechBlock(play), play);
    }
    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(3M);
}
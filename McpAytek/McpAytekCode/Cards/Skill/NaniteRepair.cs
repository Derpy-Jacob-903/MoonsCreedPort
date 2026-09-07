using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

namespace McpAytek.McpAytekCode.Cards;

public class NaniteRepair() : AytekCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self), ITechKeyword
{
    public override int CanonicalStarCost => 2;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(7m, ValueProp.Move),
        new HealVar(5)
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Exhaust
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, base.DynamicVars.Block, play);
        if (!TriggeredTech(play)) return;
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
    }
    public override bool CanBeGeneratedInCombat => false;
    protected override void OnUpgrade()
    {
        this.DynamicVars.Block.UpgradeValueBy(2M);
        this.DynamicVars.Heal.UpgradeValueBy(2M);
    }
}
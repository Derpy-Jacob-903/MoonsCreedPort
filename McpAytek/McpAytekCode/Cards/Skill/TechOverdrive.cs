using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

namespace McpAytek.McpAytekCode.Cards;

public class TechOverdrive() : AytekCard(0,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords=>
    [
        CardKeyword.Exhaust
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new TechPointVar(2),
        new EnergyVar(1)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await TechPointVar.GainTP(this);
        await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
    }

    protected override void OnUpgrade() => this.DynamicVars["TechPointVar"].UpgradeValueBy(1M);
}
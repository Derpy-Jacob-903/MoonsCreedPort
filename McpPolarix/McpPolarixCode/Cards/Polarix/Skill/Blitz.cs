using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace McpPolarix.McpPolarixCode.Cards;

public class Blitz() : PolarixCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [new CardsVar(3)];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [];

    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        
    }

    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}
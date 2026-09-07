using BaseLib.Cards.Variables;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Extensions;

namespace McpAytek.McpAytekCode.Cards;

public class DataCollection() : AytekCard(1,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    public override string CustomPortraitPath => "beta_art_2.png".CardImagePath();
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
        [CardKeyword.Exhaust, CardKeyword.Innate];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new StarsVar(1),
        new CardsVar(6),
        ..MakeCalculatedVar("CalculatedStars", 0, (card, _) => PileType.Draw.GetPile(card.Owner).Cards.Count / card.DynamicVars.Cards.BaseValue)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await PlayerCmd.GainStars(((CalculatedVar)DynamicVars["CalculatedStars"]).Calculate(null), Owner);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(-1m);
    }
}
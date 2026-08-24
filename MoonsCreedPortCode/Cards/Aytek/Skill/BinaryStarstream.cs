using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Extensions;
using MoonsCreedPort.MoonsCreedPortCode.Powers;
using static MegaCrit.Sts2.Core.Entities.Cards.PileType;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class BinaryStarstream() : AytekCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => "beta_art_11.png".CardImagePath();
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new TechPointVar(1),
        ..MakeCalculatedVar("CalculatedStars", 0, (card, _) => card.Owner == null || Hand.GetPile(card.Owner) == null ? 0 : Hand.GetPile(card.Owner).Cards.Count((c => c.IsUpgraded)))
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await PlayerCmd.GainStars(((CalculatedVar)DynamicVars["CalculatedStars"]).Calculate(null), Owner);
    }
    protected override void OnUpgrade()
    {
        this.AddKeyword(CardKeyword.Retain);
    }
}
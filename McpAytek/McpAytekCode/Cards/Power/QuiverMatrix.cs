using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Extensions;
using MoonsCreedPort.MoonsCreedPortCode.Powers.Aytek;

namespace McpAytek.McpAytekCode.Cards;

public class QuiverMatrix() : AytekCard(2,
    CardType.Power, CardRarity.Rare,
    TargetType.AnyEnemy)
{
    public override string CustomPortraitPath => "beta_art_9.png".CardImagePath();
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new TechPointVar(1)
    ];

    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Ethereal];

    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await PowerCmd.Apply<QuiverMatrixPower>(context, Owner.Creature, DynamicVars["TechPointVar"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        this.AddKeyword(CardKeyword.Innate);
    }
}
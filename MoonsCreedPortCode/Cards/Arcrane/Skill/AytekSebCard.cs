using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Arcrane;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class SebCardArcrane() : ArcraneCard(1,
    CardType.Skill, CardRarity.Ancient,
    TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Retain,
        CardKeyword.Sly,
        CardKeyword.Exhaust,
        CardKeyword.Innate
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await CardPileCmd.Draw(context, (Decimal) (CardPile.MaxCardsInHand - Owner.PlayerCombatState.Hand.Cards.Count), Owner);
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
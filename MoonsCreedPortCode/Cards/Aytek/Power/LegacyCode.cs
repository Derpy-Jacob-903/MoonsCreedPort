using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class LegacyCode() : AytekCard(1,
    CardType.Skill, CardRarity.Ancient,
    TargetType.Self)
{
    public override int CanonicalStarCost => 5;

    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Ethereal,
        CardKeyword.Exhaust
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        CardPile pile = PileType.Hand.GetPile(Owner);
        CardModel card = Owner.RunState.Rng.CombatCardSelection.NextItem(pile.Cards.Where(c => c.IsUpgradable));
        if (card == null) return;
        CardCmd.Upgrade(card);
        if (card.DeckVersion == null) return;
        CardCmd.Upgrade(card.DeckVersion);
    }

    protected override void OnUpgrade() => this.RemoveKeyword(CardKeyword.Ethereal);
}
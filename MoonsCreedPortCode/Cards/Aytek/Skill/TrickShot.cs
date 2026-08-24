using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class TrickShot() : AytekCard(1,
    CardType.Skill, CardRarity.Common,
    TargetType.RandomEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(2)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        var cards = PileType.Draw.GetPile(Owner).Cards
            .Where((Func<CardModel, bool>)(c => c.Keywords.Contains(AytekStuff.GunKeyword)))
            .Take(DynamicVars.Cards.IntValue).ToArray();
        foreach (var card in cards)
        {
            if (card.Pile?.Type != PileType.Draw) continue;
            card.ExhaustOnNextPlay = true;
            await CardCmd.AutoPlay(context, card, play.Target);
        }
    }
    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}
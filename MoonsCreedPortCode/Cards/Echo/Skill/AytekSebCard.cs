using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Cards.Echo;
using MoonsCreedPort.MoonsCreedPortCode.Character.Arcrane;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Character.Echo;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class SebCardEcho() : EchoCard(2,
    CardType.Skill, CardRarity.Ancient,
    TargetType.Self)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Sly
    ];
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        foreach (CardModel card in await ReapCard.CreateInHand(Owner, DynamicVars.Cards.IntValue, CombatState))
            CardCmd.ApplyKeyword(card, CardKeyword.Sly);
        //CardCmd.Enchant<Inky>(card, 1M);
    }
    public override async Task AfterCardDiscarded(PlayerChoiceContext choiceContext, CardModel card)
    {
        if (card.Owner != Owner || Owner.Creature.Side != Owner.Creature.CombatState.CurrentSide || Pile.Type == PileType.Hand)
            return;
        if (card != this)
        {
            await CardPileCmd.Add(this, PileType.Hand);
        }
    }
    //protected override void OnUpgrade() => up;
}
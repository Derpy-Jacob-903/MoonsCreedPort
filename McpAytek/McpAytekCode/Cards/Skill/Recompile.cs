using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

namespace McpAytek.McpAytekCode.Cards;

public class Recompile() : AytekCard(2,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(8m, ValueProp.Move)
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
        var prefs = new CardSelectorPrefs(SelectionScreenPrompt, 1);
        var card = (await CardSelectCmd.FromSimpleGrid(context, PileType.Discard.GetPile(Owner).Cards, Owner, prefs)).FirstOrDefault<CardModel>();
        if (card == null)
            return;
        await CardPileCmd.Add(card, PileType.Hand);
    }

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(4M);
}
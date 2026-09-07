using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MoonsCreedPort.MoonsCreedPortCode;

namespace McpPolarix.McpPolarixCode.Cards;

public class CovetPolarix() : PolarixCard(0,
    CardType.Skill, CardRarity.Basic,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(1)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await CardPileCmd.Draw(context, Owner);
        
    }

    /// <summary>
    /// Shorthand for the "Exhaust a Crude card" effect.
    /// </summary>
    /// <param name="context">a PlayerChoiceContext.</param>
    /// <param name="player">Should just be "this.Owner".</param>
    /// <param name="cards">How many cards to Exhaust?</param>
    /// <param name="cardModel">Should be "this".</param>
    /// <returns>How many cards were cards Exhausted?</returns>
    public static async Task<int> CovetCmd(PlayerChoiceContext context, Player player, int cards, CardModel cardModel)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, cards);
        var list = (await CardSelectCmd.FromHand(context, player, prefs, AytekStuff.IsCrude, cardModel)).ToList();
        if (list.Count == 0)
            return 0;
        var i = 0;
        foreach (CardModel card in list)
        {
            await CardCmd.Exhaust(context, card);
            i++;
        }
        return i;
    }

    protected override void OnUpgrade() => this.AddKeyword(CardKeyword.Retain);
}
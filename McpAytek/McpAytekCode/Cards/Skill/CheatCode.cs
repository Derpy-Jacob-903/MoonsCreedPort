using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

namespace McpAytek.McpAytekCode.Cards;

public class CheatCode() : AytekCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DynamicVar("Exhausts", 2),
        new DynamicVar("Discount", 1)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        CardSelectorPrefs prefs = new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, DynamicVars["Exhausts"].IntValue);
        foreach (CardModel card in await CardSelectCmd.FromHand(context, Owner, prefs, null, this))
            await CardCmd.Exhaust(context, card);
        foreach (CardModel card in PileType.Hand.GetPile(Owner).Cards)
        {
            if (!card.EnergyCost.CostsX)
                card.EnergyCost.AddThisTurn(-DynamicVars["Discount"].IntValue);
        }
    }

    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}
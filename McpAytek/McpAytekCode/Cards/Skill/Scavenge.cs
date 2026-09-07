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

public class Scavenge_Aytek() : AytekCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(2)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        var card = (await CardSelectCmd.FromHand(context, Owner, new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1), null, this)).FirstOrDefault();
        if (card != null)
            await CardCmd.Exhaust(context, card);
        await PowerCmd.Apply<EnergyNextTurnPower>(context, Owner.Creature, DynamicVars.Energy.IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}
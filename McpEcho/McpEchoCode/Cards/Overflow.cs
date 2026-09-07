using BaseLib.Utils;
using McpEcho.McpEchoCode.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;

namespace McpEcho.McpEchoCode.Cards;

[Pool(typeof(EventCardPool))]
public class Overflow() : EchoCard(1,
    CardType.Skill, CardRarity.Event, //UC
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(6) //may buff
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        var baseValue = DynamicVars.Cards.BaseValue;
        if (Owner.PlayerCombatState != null)
        {
            var count = Owner.PlayerCombatState.Hand.Cards.Count;
            await CardPileCmd.Draw(context, Math.Max(0M, baseValue - count), Owner);
        }
    }
    
    public override CardPoolModel VisualCardPool => ModelDb.CardPool<EchoCardPool>();

    protected override void OnUpgrade() => this.DynamicVars.Cards.UpgradeValueBy(1M);
}
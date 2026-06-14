using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MoonsCreedPort.MoonsCreedPortCode.Character.Echo;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Echo;

public class DarkThoughts() : EchoCard(2,
    CardType.Status, CardRarity.Status,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(2)
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        CardKeyword.Sly
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await CardPileCmd.Draw(context, DynamicVars.Cards.IntValue, Owner);
    }
    //public override CardPoolModel VisualCardPool => ModelDb.CardPool<EchoCardPool>();
    protected override void OnUpgrade() => DynamicVars.Cards.UpgradeValueBy(1);
}
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MoonsCreedPort.MoonsCreedPortCode.Character.Echo;
using MoonsCreedPort.MoonsCreedPortCode.Powers.Echo;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Echo;

[Pool(typeof(EventCardPool))]
public class EclipseEcho() : EchoCard(1,
    CardType.Power, CardRarity.Event, //Rare? maybe UC
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<OldWellLaidPlansPower>(1)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await PowerCmd.Apply<OldWellLaidPlansPower>(context, Owner.Creature, DynamicVars.Strength.BaseValue, Owner.Creature, this);
    }

    public override CardPoolModel VisualCardPool => ModelDb.CardPool<EchoCardPool>();

    protected override void OnUpgrade() => this.DynamicVars["OldWellLaidPlansPower"].UpgradeValueBy(1M);
}
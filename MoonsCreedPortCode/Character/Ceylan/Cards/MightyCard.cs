using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MoonsCreedPort.MoonsCreedPortCode.Character.Polarix;
using MoonsCreedPort.MoonsCreedPortCode.Powers.Aytek;

namespace MoonsCreedPort.MoonsCreedPortCode.Character.Aytek.Cards;

[Pool(typeof(EventCardPool))]
public class MightyCard() : ColorlessCard(1,
    CardType.Skill, CardRarity.Event,
    TargetType.Self)
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<MightyPower>()];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<MightyPower>(3)
    ];
    
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await CommonActions.ApplySelf<MightyPower>(context, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["MightyPower"].UpgradeValueBy(2M);
    }

    public override CardPoolModel VisualCardPool => ModelDb.CardPool<PolarixCardPool>();
}
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Extensions;
using MoonsCreedPort.MoonsCreedPortCode.Powers.Aytek;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class KingsArtillery() : AytekCard(0,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    public override string CustomPortraitPath => "beta_art_5.png".CardImagePath();
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<KingsArtilleryPower>(1)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await CommonActions.ApplySelf<KingsArtilleryPower>(context, this);
    }

    protected override void OnUpgrade() {
        this.AddKeyword(CardKeyword.Innate);
    }
}
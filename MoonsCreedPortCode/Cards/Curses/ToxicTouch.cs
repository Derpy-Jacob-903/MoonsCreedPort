using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MoonsCreedPort.MoonsCreedPortCode.Character;
using MoonsCreedPort.MoonsCreedPortCode.Character.Polarix;
using MoonsCreedPort.MoonsCreedPortCode.Extensions;
using MoonsCreedPort.MoonsCreedPortCode.Powers.Enemy;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Polarix;

[Pool(typeof(StatusCardPool))]
public class ToxicTouch() : CollarlessCard(1,
    CardType.Status, CardRarity.Status,
    TargetType.Self)
{
    public override int MaxUpgradeLevel => 0;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<VenomPower>(3)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<VenomPower>()
    ];

    public override async Task AfterCardDrawn(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool fromHandDraw)
    {
        var @void = this;
        if (card != @void)
            return;
        await Cmd.Wait(0.25f);
        await PowerCmd.Apply<VenomPower>(choiceContext, Owner.Creature, DynamicVars["VenomPower"].BaseValue, null, this);
    }
}
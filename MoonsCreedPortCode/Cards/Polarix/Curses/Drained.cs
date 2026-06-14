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

[Pool(typeof(CurseCardPool))]
public class Drained() : ColorlessCard(-1,
    CardType.Curse, CardRarity.Curse,
    TargetType.Self)
{public override int MaxUpgradeLevel => 0;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new EnergyVar(1)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.ForEnergy(this)
    ];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords=>
    [
        CardKeyword.Unplayable,
        //CardKeyword.Exhaust,
        CardKeyword.Ethereal
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
        await PlayerCmd.LoseEnergy(@void.DynamicVars.Energy.IntValue, @void.Owner);
    }
}
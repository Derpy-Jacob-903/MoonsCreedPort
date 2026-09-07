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
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MoonsCreedPort.MoonsCreedPortCode.Character;
using MoonsCreedPort.MoonsCreedPortCode.Extensions;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Polarix;

[Pool(typeof(StatusCardPool))]
public class Haunt() : CollarlessCard(1,
    CardType.Status, CardRarity.Status,
    TargetType.Self)
{
    public override int MaxUpgradeLevel => 0;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<VulnerablePower>(2)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<VulnerablePower>()
    ];

    public override bool HasTurnEndInHandEffect => true;

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        await PowerCmd.Apply<VulnerablePower>(choiceContext, Owner.Creature, DynamicVars.Vulnerable.BaseValue, null, this);
    }
}
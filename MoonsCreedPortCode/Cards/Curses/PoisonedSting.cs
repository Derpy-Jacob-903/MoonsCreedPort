using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MoonsCreedPort.MoonsCreedPortCode.Character;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Polarix;

[Pool(typeof(StatusCardPool))]
public class PoisonedSting() : CollarlessCard(1,
    CardType.Status, CardRarity.Status,
    TargetType.Self)
{
    public override int MaxUpgradeLevel => 0;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<ThornsPower>(1)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<ThornsPower>()
    ];

    public override bool HasTurnEndInHandEffect => true;

    protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
    {
        if (CombatState == null) return;
        foreach (var creature in CombatState.Enemies)
        {
            await PowerCmd.Apply<ThornsPower>(choiceContext, Owner.Creature, DynamicVars["ThornsPower"].BaseValue, null, this);
        }
        /*var target = CombatState.Enemies.FirstOrDefault(e => e.Monster is AbstractScorpio) ?? CombatState.Enemies[0];
        if (target is null) return;
        await PowerCmd.Apply<WeakPower>(choiceContext, Owner.Creature, DynamicVars.Weak.BaseValue, null, this);*/
    }
}
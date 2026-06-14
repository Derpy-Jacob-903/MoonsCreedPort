using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class SystemReboot() : AytekCard(3,
    CardType.Skill, CardRarity.Rare,
    TargetType.Self)
{
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<DrawCardsNextTurnPower>("DrawCardsNextTurn", 2),
        new PowerVar<EnergyNextTurnPower>("EnergyNextTurn", 2)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await PowerCmd.Apply<DrawCardsNextTurnPower>(context, Owner.Creature, DynamicVars["DrawCardsNextTurn"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<EnergyNextTurnPower>(context, Owner.Creature, DynamicVars["EnergyNextTurn"].BaseValue, Owner.Creature, this);
        PlayerCmd.EndTurn(Owner, false);
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars["DrawCardsNextTurn"].UpgradeValueBy(1M);
        this.EnergyCost.UpgradeBy(-1);
    } 
}
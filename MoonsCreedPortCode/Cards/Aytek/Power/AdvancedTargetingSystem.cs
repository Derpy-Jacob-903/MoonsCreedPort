using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Powers;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Powers.Aytek;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class AdvancedTargetingSystem() : AytekCard(2,
    CardType.Power, CardRarity.Uncommon,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<TargetingSystemPower>(4)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await PowerCmd.Apply<TargetingSystemPower>(context, Owner.Creature, DynamicVars["TargetingSystemPower"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}
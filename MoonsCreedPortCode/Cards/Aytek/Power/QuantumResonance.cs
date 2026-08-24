using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Powers;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Powers.Aytek;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class QuantumResonance() : AytekCard(1,
    CardType.Power, CardRarity.Rare,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<QuantumResonancePower>(5)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await PowerCmd.Apply<QuantumResonancePower>(context, Owner.Creature, DynamicVars["QuantumResonancePower"].BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade() => this.DynamicVars["QuantumResonancePower"].UpgradeValueBy(3M);
}
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Powers.Aytek;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class Overload() : AytekCard(0,
    CardType.Skill, CardRarity.Common,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new TechPointVar(3),
        new PowerVar<HpLossNextTurnPower>("HpLossNextTurnPower", 3)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        await TechPointVar.GainTP(this);
        await PowerCmd.Apply<HpLossNextTurnPower>(context, Owner.Creature, DynamicVars["HpLossNextTurnPower"].BaseValue, Owner.Creature,
            this);
    }

    protected override void OnUpgrade() => this.DynamicVars["TechPointVar"].UpgradeValueBy(2M);
}
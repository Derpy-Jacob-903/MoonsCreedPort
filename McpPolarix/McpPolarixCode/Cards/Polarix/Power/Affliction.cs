using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Powers;

namespace McpPolarix.McpPolarixCode.Cards;

public class Affliction() : PolarixCard(0,
    CardType.Power, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<SleightOfFleshPower>("Thorns", 3)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        Log.Warn(this.Id.Entry + ": This card is unimplemented!!");
        await PowerCmd.Apply<SleightOfFleshPower>(context, Owner.Creature, DynamicVars["Thorns"].BaseValue, Owner.Creature, this);
    }

    protected override void OnUpgrade() => this.DynamicVars["Thorns"].UpgradeValueBy(1M);
}
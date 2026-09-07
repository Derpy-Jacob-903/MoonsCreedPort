using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MoonsCreedPort.MoonsCreedPortCode.Powers.Echo;

namespace McpEcho.McpEchoCode.Cards;

public class SellYourSoul() : EchoCard(0,
    CardType.Power, CardRarity.Ancient,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<RitualPower>( 3)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await PowerCmd.Apply<RitualPower>(context, Owner.Creature, DynamicVars["RitualPower"].BaseValue, Owner.Creature, this);
        await PowerCmd.Apply<SoldSoulPower>(context, Owner.Creature, 1, Owner.Creature, this);
    }

    protected override void OnUpgrade() => this.DynamicVars["RitualPower"].UpgradeValueBy(1M);
}
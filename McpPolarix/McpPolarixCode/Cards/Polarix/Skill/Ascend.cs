using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace McpPolarix.McpPolarixCode.Cards;

public class Ascend() : PolarixCard(1,
    CardType.Skill, CardRarity.Ancient,
    TargetType.AnyEnemy)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await PowerCmd.Apply<StrengthPower>(context, Owner.Creature, Owner.Creature.GetPowerAmount<StrengthPower>(), Owner.Creature, this);
    }

    protected override bool ShouldGlowRedInternal => (Owner.Creature.GetPowerAmount<StrengthPower>() <= 1);

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(3M);
}
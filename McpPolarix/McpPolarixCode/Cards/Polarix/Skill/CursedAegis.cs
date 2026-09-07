using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode;

namespace McpPolarix.McpPolarixCode.Cards;

public class CursedAegis() : PolarixCard(0,
    CardType.Skill, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        ..MakeCalculatedBlock(0, (card, creature) => PileType.Hand.GetPile(card.Owner).Cards.Count((Func<CardModel, bool>) AytekStuff.IsCrude), 5)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, new BlockVar(DynamicVars.CalculatedBlock.BaseValue, ValueProp.Move), play);
    }

    protected override void OnUpgrade() => this.DynamicVars.CalculationExtra.UpgradeValueBy(3M);
}
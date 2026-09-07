using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;

namespace McpPolarix.McpPolarixCode.Cards;

public class PowerThrough() : PolarixCard(1,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(13m, ValueProp.Move),
        new RepeatVar(2)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, base.DynamicVars.Block, play);
        for (var i = 0; i < DynamicVars.Repeat.IntValue; i++)
        {
            CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Wound>(Owner), PileType.Draw, Owner));
            await Cmd.Wait(0.5f);
        }
    }

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(3M);
}
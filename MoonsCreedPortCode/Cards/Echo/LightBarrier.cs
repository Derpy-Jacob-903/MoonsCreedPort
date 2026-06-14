
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Echo;
using MoonsCreedPort.MoonsCreedPortCode.Powers;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Echo;

public class LightBarrier() : EchoCard(0,
    CardType.Skill, CardRarity.Basic,
    TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[]
        {
            new BlockVar(3m, ValueProp.Move),
            new CalculationBaseVar(0M),
            new CalculationExtraVar(1M),
            new CalculatedVar("CalculatedHits").WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => card.Owner.PlayerCombatState.OrbQueue.Orbs.Count((Func<OrbModel, bool>) (c => c is WhiteEchoOrb))))
        };
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.Static(StaticHoverTip.Block),
        HoverTipFactory.FromOrb<WhiteEchoOrb>(),
        HoverTipFactory.Static(StaticHoverTip.Evoke)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        for (int i = 0; i < ((CalculatedVar)DynamicVars["CalculatedHits"]).Calculate(null); i++)
        {
            await CreatureCmd.GainBlock(Owner.Creature, base.DynamicVars.Block, play);
        }
        var times = await EchoOrb<PowerModel>.EvokeAllOf<WhiteEchoOrb>(context, Owner);
    }
    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(1M);
}
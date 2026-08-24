using BaseLib.Utils;
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
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class EmpDisruptor() : AytekCard(3,
    CardType.Skill, CardRarity.Uncommon,
    TargetType.Self), ITechKeyword
{
    public override int CanonicalStarCost => 2;
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new PowerVar<WeakPower>(2),
        new PowerVar<VulnerablePower>(2),
        new EnergyVar(2)
    ];
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromPower<WeakPower>(),
        HoverTipFactory.FromPower<VulnerablePower>(),
        //HoverTipFactory.()
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (CombatState == null) return;
        foreach (Creature enemy in (IEnumerable<Creature>) CombatState.HittableEnemies)
        {
            WeakPower weakPower = await PowerCmd.Apply<WeakPower>(context, enemy, DynamicVars.Weak.BaseValue, Owner.Creature, this);
            VulnerablePower vulnerablePower = await PowerCmd.Apply<VulnerablePower>(context, enemy, DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);
        }
        if (!TriggeredTech(play)) return;
        
    }

    protected override void OnUpgrade()
    {
        this.DynamicVars.Vulnerable.UpgradeValueBy(1M);
        this.DynamicVars.Weak.UpgradeValueBy(1M);
    }
}
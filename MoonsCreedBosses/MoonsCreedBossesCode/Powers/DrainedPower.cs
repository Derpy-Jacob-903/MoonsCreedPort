using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.HoverTips;

namespace MoonsCreedBosses.MoonsCreedBossesCode.Powers;

public class DrainedPower : MoonsCreedBossesPower
{
    public override async Task AfterEnergyReset(Player player)
    {
        if (player != Owner.Player) return;
        await PlayerCmd.LoseEnergy(Amount, player);
        await PowerCmd.Remove(this);
    }

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.Energy)];

    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
}
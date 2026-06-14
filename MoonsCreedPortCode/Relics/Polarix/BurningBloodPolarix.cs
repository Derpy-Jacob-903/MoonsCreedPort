using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Character.Polarix;

namespace MoonsCreedPort.MoonsCreedPortCode.Relics.Polarix;

public class BurningBloodPolarix() : PolarixRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;
    
    public override RelicModel GetUpgradeReplacement()
    {
        return ModelDb.Relic<BlackBlood>();
    }
    
    protected override string PackedIconOutlinePath => ImageHelper.GetImagePath($"atlases/relic_outline_atlas.sprites/burning_blood.tres");
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new HealVar(6M)
    ];
    
    public override async Task AfterCombatVictory(CombatRoom _)
    {
        if (Owner.Creature.IsDead)
            return;
        Flash();
        await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
    }
}
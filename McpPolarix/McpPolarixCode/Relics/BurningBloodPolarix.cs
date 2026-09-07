using BaseLib.Utils;
using HarmonyLib;
using McpPolarix.McpPolarixCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MegaCrit.Sts2.Core.Rooms;

namespace McpPolarix.McpPolarixCode.Relics;

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


    [HarmonyPatch(nameof(CardModel.IsBasicStrikeOrDefend), MethodType.Getter)]
    public static class Card_IsBasicStrikeOrDefend_Patch
    {
        [HarmonyPostfix]
        static void Postfix(CardModel __instance, ref bool __result)
        {
            if (__instance is RadiantStrike)
                __result = false;
        }
    }
}
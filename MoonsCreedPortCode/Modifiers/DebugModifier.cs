using BaseLib.Abstracts;
using HarmonyLib;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Encounters;
using MegaCrit.Sts2.Core.Models.Modifiers;
using MegaCrit.Sts2.Core.Rooms;
using MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek.Cards;

namespace MoonsCreedPort.MoonsCreedPortCode.Modifiers;

public class DebugModifier : CustomModifierModel
{
    public override ModifierAlignment Alignment => ModifierAlignment.Bad;
    
    [HarmonyPatch(typeof(ActModel))]
    public static class TrashHeapPatch
    {
        [HarmonyPatch("AllWeakEncounters", MethodType.Getter)]
        [HarmonyPostfix]
        public static void Postfix1(ActModel __instance, ref EncounterModel[] __result)
        {
            if (!__instance.Run.Modifiers.Any(m => m is FortuneFavorsTheBold))
            {
                return true;
            }
            __result = [ ModelDb.Encounter<PhantasmalGardenersElite>() ];
        }
        [HarmonyPatch("AllRegularEncounters", MethodType.Getter)]
        [HarmonyPostfix]
        public static void Postfix2(ActModel __instance, ref EncounterModel[] __result)
        {
            __result = [ ModelDb.Encounter<PhantasmalGardenersElite>() ];
        }
        [HarmonyPatch("AllEliteEncounters", MethodType.Getter)]
        [HarmonyPostfix]
        public static void Postfix3(ActModel __instance, ref EncounterModel[] __result)
        {
            __result = [ ModelDb.Encounter<PhantasmalGardenersElite>() ];
        }
    }
}
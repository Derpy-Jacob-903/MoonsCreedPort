using HarmonyLib;
using McpPolarix.McpPolarixCode.Cards;
using McpPolarix.McpPolarixCode.Character;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards;

[HarmonyPatch(typeof(ColorfulPhilosophers))]
public static class ColorfulPhilosophersPatch
{
    [HarmonyPatch("CardPoolColorOrder", MethodType.Getter)]
    [HarmonyPostfix]
    public static void Postfix(ref IEnumerable<CardPoolModel> __result)
    {
        __result = __result.Append(ModelDb.CardPool<PolarixCardPool>());
    }
}

[HarmonyPatch(typeof(TrashHeap))]
public static class TrashHeapPatch
{
    [HarmonyPatch("Cards", MethodType.Getter)]
    [HarmonyPostfix]
    public static void Postfix(ref CardModel[] __result)
    {
        __result = __result.AddToArray(ModelDb.Card<MightyCard>());
    }
}
using HarmonyLib;
using McpEcho.McpEchoCode.Cards;
using McpEcho.McpEchoCode.Character;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;

namespace McpEcho.McpEchoCode;

[HarmonyPatch(typeof(ColorfulPhilosophers))]
public static class ColorfulPhilosophersPatch
{
    [HarmonyPatch("CardPoolColorOrder", MethodType.Getter)]
    [HarmonyPostfix]
    public static void Postfix(ref IEnumerable<CardPoolModel> __result)
    {
        __result = __result.Append(ModelDb.CardPool<EchoCardPool>());
    }
}

[HarmonyPatch(typeof(TrashHeap))]
public static class TrashHeapPatch
{
    [HarmonyPatch("Cards", MethodType.Getter)]
    [HarmonyPostfix]
    public static void Postfix(ref CardModel[] __result)
    {
        __result = __result.AddToArray(ModelDb.Card<Overflow>());
        //__result = __result.AddToArray(ModelDb.Card<EclipseEcho>());
    }
}
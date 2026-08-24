using System.Diagnostics.CodeAnalysis;
using HarmonyLib;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Events;
using MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Cards.Echo;
using MoonsCreedPort.MoonsCreedPortCode.Cards.Polarix;
using MoonsCreedPort.MoonsCreedPortCode.Character.Arcrane;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek.Cards;
using MoonsCreedPort.MoonsCreedPortCode.Character.Echo;
using MoonsCreedPort.MoonsCreedPortCode.Character.Polarix;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards;

[HarmonyPatch(typeof(ColorfulPhilosophers))]
public static class ColorfulPhilosophersPatch
{
    [HarmonyPatch("CardPoolColorOrder", MethodType.Getter)]
    [HarmonyPostfix]
    public static void Postfix(ref IEnumerable<CardPoolModel> __result)
    {
        __result = __result.Append(ModelDb.CardPool<AytekCardPool>());
        __result = __result.Append(ModelDb.CardPool<EchoCardPool>());
        __result = __result.Append(ModelDb.CardPool<PolarixCardPool>());
        //__result = __result.Append(ModelDb.CardPool<ArcraneCardPool>());
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
        __result = __result.AddToArray(ModelDb.Card<Overflow>());
        //__result = __result.AddToArray(ModelDb.Card<EclipseEcho>());
        __result = __result.AddToArray(ModelDb.Card<ShieldMelter>());
    }
}
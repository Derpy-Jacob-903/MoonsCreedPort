using System.Collections;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace MoonsCreedBosses.MoonsCreedBossesCode;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "MoonsCreedBosses"; //Used for resource filepath
    public const string ResPath = $"res://{ModId}";

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        //If you want to use scripts defined in your mod for Godot scenes, uncomment the following line.
        //Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(Assembly.GetExecutingAssembly());

        Harmony harmony = new(ModId);
        harmony.PatchAll();
        
        TryRegisterCreatureType();
    }
    
    public static void TryRegisterCreatureType()
    {
        Logger.Info("Attempting CreatureCapture creature registration...");

        var registry = AccessTools.TypeByName(
            "CreatureCapture.CreatureCaptureCode.Types.CreatureTypeRegistry"
        );

        if (registry == null)
        {
            Logger.Info("FAILED: CreatureTypeRegistry not found.");
            return;
        }

        Logger.Info($"Found registry: {registry.AssemblyQualifiedName}");

        var exactField = AccessTools.Field(registry, "Exact");

        if (exactField == null)
        {
            Logger.Info("FAILED: Exact field not found.");
            return;
        }

        if (exactField.GetValue(null) is not IDictionary exact)
        {
            Logger.Info("FAILED: Exact is null/not IDictionary.");
            return;
        }

        Logger.Info($"Found Exact dictionary with {exact.Count} entries.");

        var creatureType = AccessTools.TypeByName(
            "CreatureCapture.CreatureCaptureCode.Types.CreatureType"
        );

        if (creatureType == null)
        {
            Logger.Info("FAILED: CreatureType not found.");
            return;
        }

        Logger.Info($"Found CreatureType: {creatureType.AssemblyQualifiedName}");
    var air = Enum.Parse(creatureType, "Electric");
    var fire = Enum.Parse(creatureType, "Fire");
    var water = Enum.Parse(creatureType, "Water");
    var earth = Enum.Parse(creatureType, "Earth");

    exact["MOONSCREEDBOSSES-AQUARIUS_ONE"] = air;
    exact["MOONSCREEDBOSSES-AQUARIUS_TWO"] = air;
    exact["MOONSCREEDBOSSES-AQUARIUS_THREE"] = air;
    exact["MOONSCREEDBOSSES-ARIES_ONE"] = fire;
    exact["MOONSCREEDBOSSES-ARIES_TWO"] = fire;
    exact["MOONSCREEDBOSSES-ARIES_THREE"] = fire;
    exact["MOONSCREEDBOSSES-CANCER_ONE"] = water;
    exact["MOONSCREEDBOSSES-CANCER_TWO"] = water;
    exact["MOONSCREEDBOSSES-CANCER_THREE"] = water;
    exact["MOONSCREEDBOSSES-CAPRICORN_ONE"] = earth;
    exact["MOONSCREEDBOSSES-CAPRICORN_TWO"] = earth;
    exact["MOONSCREEDBOSSES-CAPRICORN_THREE"] = earth;
    exact["MOONSCREEDBOSSES-GEMINI_A_ONE"] = air;
    exact["MOONSCREEDBOSSES-GEMINI_A_TWO"] = air;
    exact["MOONSCREEDBOSSES-GEMINI_A_THREE"] = air;
    exact["MOONSCREEDBOSSES-GEMINI_B_ONE"] = air;
    exact["MOONSCREEDBOSSES-GEMINI_B_TWO"] = air;
    exact["MOONSCREEDBOSSES-GEMINI_B_THREE"] = air;
    exact["MOONSCREEDBOSSES-LEO_ONE"] = fire;
    exact["MOONSCREEDBOSSES-LEO_TWO"] = fire;
    exact["MOONSCREEDBOSSES-LEO_THREE"] = fire;
    exact["MOONSCREEDBOSSES-SCORPIO_ONE"] = water;
    exact["MOONSCREEDBOSSES-SCORPIO_TWO"] = water;
    exact["MOONSCREEDBOSSES-SCORPIO_THREE"] = water;
    exact["MOONSCREEDBOSSES-VIRGO_ONE"] = earth;
    exact["MOONSCREEDBOSSES-VIRGO_TWO"] = earth;
    exact["MOONSCREEDBOSSES-VIRGO_THREE"] = earth;
    exact["MOONSCREEDBOSSES-LIBRA_ONE"] = air;
    exact["MOONSCREEDBOSSES-LIBRA_TWO"] = air;
    exact["MOONSCREEDBOSSES-LIBRA_THREE"] = air;
    exact["MOONSCREEDBOSSES-SAGITTARIUS_ONE"] = fire;
    exact["MOONSCREEDBOSSES-SAGITTARIUS_TWO"] = fire;
    exact["MOONSCREEDBOSSES-SAGITTARIUS_THREE"] = fire;
    exact["MOONSCREEDBOSSES-PISCES_ONE"] = water;
    exact["MOONSCREEDBOSSES-PISCES_TWO"] = water;
    exact["MOONSCREEDBOSSES-PISCES_THREE"] = water;
    exact["MOONSCREEDBOSSES-TAURUS_ONE"] = earth;
    exact["MOONSCREEDBOSSES-TAURUS_TWO"] = earth;
    exact["MOONSCREEDBOSSES-TAURUS_THREE"] = earth;
    }
}

public class Patch : SingletonModel
{
    /*[HarmonyPatch(typeof(ActModel), nameof(ActModel._allBossEncounters))]
    public static class PlayerCmdLoseEnergyPatch
    {
        private static void Postfix(ActModel __instance)
        {
        }
    }*/

    public override bool ShouldReceiveCombatHooks => true;
}
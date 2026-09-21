using Collector.CollectorCode.Core;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;

namespace McpZodiacCollectibles.McpZodiacCollectiblesCode;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    public const string ModId = "McpZodiacCollectibles"; //Used for resource filepath
    public const string ResPath = $"res://{ModId}";

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        //If you want to use scripts defined in your mod for Godot scenes, uncomment the following line.
        //Godot.Bridge.ScriptManagerBridge.LookupScriptsInAssembly(Assembly.GetExecutingAssembly());

        Harmony harmony = new(ModId);

        harmony.PatchAll();
    }

    [HarmonyPatch(typeof(CollectorCmd))]
    public static class CollectorCmdPatch
    {
        //Patching this so Act 2 and 3 Zodiacs share cards with their Act 1 version.
        [HarmonyPatch(nameof(CollectorCmd.GetCardForModdedEnemy))]
        [HarmonyPrefix]
        public static bool OnPlayPrefix(CardModel __instance, ModelId encounterId)
        {
            var moddedEnemyMap = new Dictionary<string, string>
            {
                { "RUINA2-ARIES_TWO_ENCOUNTER", "MCPZODIACCOLLECTIBLES-ARIES" },
                { "RUINA2-ARIES_THREE_ENCOUNTER", "MCPZODIACCOLLECTIBLES-ARIES" },
                { "RUINA2-TAURUS_TWO_ENCOUNTER", "MCPZODIACCOLLECTIBLES-TAURUS" },
                { "RUINA2-TAURUS_THREE_ENCOUNTER", "MCPZODIACCOLLECTIBLES-TAURUS" },
                { "RUINA2-GEMINI_TWO_ENCOUNTER", "MCPZODIACCOLLECTIBLES-GEMINI" },
                { "RUINA2-GEMINI_THREE_ENCOUNTER", "MCPZODIACCOLLECTIBLES-GEMINI" },
                { "RUINA2-CANCER_TWO_ENCOUNTER", "MCPZODIACCOLLECTIBLES-CANCER" },
                { "RUINA2-CANCER_THREE_ENCOUNTER", "MCPZODIACCOLLECTIBLES-CANCER" },
                { "RUINA2-LEO_TWO_ENCOUNTER", "MCPZODIACCOLLECTIBLES-LEO" },
                { "RUINA2-LEO_THREE_ENCOUNTER", "MCPZODIACCOLLECTIBLES-LEO" },
                { "RUINA2-VIRGO_TWO_ENCOUNTER", "MCPZODIACCOLLECTIBLES-VIRGO" },
                { "RUINA2-VIRGO_THREE_ENCOUNTER", "MCPZODIACCOLLECTIBLES-VIRGO" },
                { "RUINA2-LIBRA_TWO_ENCOUNTER", "MCPZODIACCOLLECTIBLES-LIBRA" },
                { "RUINA2-LIBRA_THREE_ENCOUNTER", "MCPZODIACCOLLECTIBLES-LIBRA" },
                { "RUINA2-SCORPIO_TWO_ENCOUNTER", "MCPZODIACCOLLECTIBLES-SCORPIO" },
                { "RUINA2-SCORPIO_THREE_ENCOUNTER", "MCPZODIACCOLLECTIBLES-SCORPIO" },
                { "RUINA2-SAGITTARIUS_TWO_ENCOUNTER", "MCPZODIACCOLLECTIBLES-SAGITTARIUS" },
                { "RUINA2-SAGITTARIUS_THREE_ENCOUNTER", "MCPZODIACCOLLECTIBLES-SAGITTARIUS" },
                { "RUINA2-CAPRICORN_TWO_ENCOUNTER", "MCPZODIACCOLLECTIBLES-CAPRICORN" },
                { "RUINA2-CAPRICORN_THREE_ENCOUNTER", "MCPZODIACCOLLECTIBLES-CAPRICORN" },
                { "RUINA2-AQUARIUS_TWO_ENCOUNTER", "MCPZODIACCOLLECTIBLES-AQUARIUS" },
                { "RUINA2-AQUARIUS_THREE_ENCOUNTER", "MCPZODIACCOLLECTIBLES-AQUARIUS" },
                { "RUINA2-PISCES_TWO_ENCOUNTER", "MCPZODIACCOLLECTIBLES-PISCES" },
                { "RUINA2-PISCES_THREE_ENCOUNTER", "MCPZODIACCOLLECTIBLES-PISCES" },
            };
            if (!moddedEnemyMap.TryGetValue(encounterId.Entry, out var value)) return true;
            var id = new ModelId("CARD", value);
            var card = ModelDb.GetByIdOrNull<CardModel>(id);
            return false;
        }
    }

}
using BaseLib.Config;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Modding;
using MoonsCreedPort.MoonsCreedPortCode.Data;

namespace MoonsCreedPort;

[ModInitializer(nameof(Initialize))]
public partial class MoonsCreedPortMainFile : Node
{
    public const string ModId = "MoonsCreedPort"; //At the moment, this is used only for the Logger and harmony names.

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } =
        new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        ModManager.OnMetricsUpload += DownfallMetrics.OnMetricsUpload;
        Harmony harmony = new(ModId);
        harmony.PatchAll();
    }
    public class ActsFromThePastConfig : SimpleModConfig
    {
        [ConfigHoverTip]
        public static bool RebalancedMode { get; set; } = false;
    }
}
using BaseLib.Abstracts;
using BaseLib.Utils;
using McpEcho.McpEchoCode.Character;

namespace McpEcho.McpEchoCode.Potions;

[Pool(typeof(McpEchoPotionPool))]
public abstract class McpEchoPotion : CustomPotionModel;
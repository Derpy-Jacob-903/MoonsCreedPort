using BaseLib.Abstracts;
using BaseLib.Utils;
using McpEcho.McpEchoCode.Character;

namespace McpEcho.McpEchoCode.Potions;

[Pool(typeof(EchoPotionPool))]
public abstract class EchoPotion : CustomPotionModel;
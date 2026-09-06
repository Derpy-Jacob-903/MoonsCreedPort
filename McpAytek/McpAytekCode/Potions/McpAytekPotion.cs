using BaseLib.Abstracts;
using BaseLib.Utils;
using McpAytek.McpAytekCode.Character;

namespace McpAytek.McpAytekCode.Potions;

[Pool(typeof(McpAytekPotionPool))]
public abstract class McpAytekPotion : CustomPotionModel;
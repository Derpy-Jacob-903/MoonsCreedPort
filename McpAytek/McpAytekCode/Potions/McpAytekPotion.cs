using BaseLib.Abstracts;
using BaseLib.Utils;
using McpAytek.McpAytekCode.Character;

namespace McpAytek.McpAytekCode.Potions;

[Pool(typeof(AytekPotionPool))]
public abstract class McpAytekPotion : CustomPotionModel;
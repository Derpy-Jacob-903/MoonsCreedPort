using BaseLib.Abstracts;
using BaseLib.Utils;
using McpPolarix.McpPolarixCode.Character;

namespace McpPolarix.McpPolarixCode.Potions;

[Pool(typeof(McpPolarixPotionPool))]
public abstract class McpPolarixPotion : CustomPotionModel;
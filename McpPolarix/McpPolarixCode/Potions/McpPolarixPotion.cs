using BaseLib.Abstracts;
using BaseLib.Utils;
using McpPolarix.McpPolarixCode.Character;

namespace McpPolarix.McpPolarixCode.Potions;

[Pool(typeof(PolarixPotionPool))]
public abstract class PolarixPotion : CustomPotionModel;
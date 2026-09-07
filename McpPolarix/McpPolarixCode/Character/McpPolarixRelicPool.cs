using BaseLib.Abstracts;
using McpPolarix.McpPolarixCode.Extensions;
using Godot;

namespace McpPolarix.McpPolarixCode.Character;

public class PolarixRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => Polarix.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}
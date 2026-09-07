using BaseLib.Abstracts;
using McpAytek.McpAytekCode.Extensions;
using Godot;

namespace McpAytek.McpAytekCode.Character;

public class AytekRelicPool : CustomRelicPoolModel
{
    public override Color LabOutlineColor => Aytek.Color;

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}
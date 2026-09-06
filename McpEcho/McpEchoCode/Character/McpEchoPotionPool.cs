using BaseLib.Abstracts;
using McpEcho.McpEchoCode.Extensions;
using Godot;

namespace McpEcho.McpEchoCode.Character;

public class McpEchoPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => McpEcho.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}
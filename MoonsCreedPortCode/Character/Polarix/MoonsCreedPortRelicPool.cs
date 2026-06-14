using BaseLib.Abstracts;
using Godot;

namespace MoonsCreedPort.MoonsCreedPortCode.Character.Polarix;

public class PolarixRelicPool : CustomRelicPoolModel
{
    public override string EnergyColorName => Polarix.CharacterId;
    public override Color LabOutlineColor => Polarix.Color;
}
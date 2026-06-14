using BaseLib.Abstracts;
using Godot;

namespace MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

public class AytekRelicPool : CustomRelicPoolModel
{
    public override string EnergyColorName => Aytek.CharacterId;
    public override Color LabOutlineColor => Aytek.Color;
}
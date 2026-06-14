using BaseLib.Abstracts;
using Godot;

namespace MoonsCreedPort.MoonsCreedPortCode.Character.Arcrane;

public class ArcraneRelicPool : CustomRelicPoolModel
{
    public override string EnergyColorName => Arcrane.CharacterId;
    public override Color LabOutlineColor => Arcrane.Color;
}
using BaseLib.Abstracts;
using Godot;

namespace MoonsCreedPort.MoonsCreedPortCode.Character.Echo;

public class EchoRelicPool : CustomRelicPoolModel
{
    public override string EnergyColorName => Echo.CharacterId;
    public override Color LabOutlineColor => Echo.Color;
}
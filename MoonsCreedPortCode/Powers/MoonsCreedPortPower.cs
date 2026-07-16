using BaseLib.Abstracts;
using BaseLib.Extensions;
using MoonsCreedPort.MoonsCreedPortCode.Extensions;

namespace MoonsCreedPort.MoonsCreedPortCode.Powers;

public abstract class MoonsCreedPortPower : CustomPowerModel
{
    //Loads from MoonsCreedPort/images/powers/your_power.png
    public override string CustomPackedIconPath => 
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
}
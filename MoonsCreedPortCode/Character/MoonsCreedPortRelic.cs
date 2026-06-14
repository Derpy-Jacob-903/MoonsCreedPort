using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MoonsCreedPort.MoonsCreedPortCode.Extensions;

namespace MoonsCreedPort.MoonsCreedPortCode.Character;

[Pool(typeof(SharedRelicPool))]
public abstract class MoonsCreedSharedRelic : CustomRelicModel
{
    public override string PackedIconPath => ResourceLoader.Exists($"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath()) ? $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath() : $"relic.png".RelicImagePath();
    protected override string PackedIconOutlinePath => ResourceLoader.Exists($"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath()) ? $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath() : $"relic_outline.png".RelicImagePath();
    protected override string BigIconPath => !ResourceLoader.Exists($"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath()) ? PackedIconPath : $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
}
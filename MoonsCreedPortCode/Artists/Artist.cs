using BaseLib.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;

namespace Downfall.DownfallCode.Artists;

public abstract class Artist
{
    private static readonly Dictionary<Type, Artist> Instances = new();

    private string Id => GetType().GetPrefix() + StringHelper.Slugify(GetType().Name);
    private static LocString ArtByLocString => new("artists", "ART_BY");
    private LocString Name => new("artists", $"{Id}.name");

    private LocString ArtByName
    {
        get
        {
            var text = ArtByLocString;
            text.Add("name", Name.GetFormattedText());
            return text;
        }
    }

    private Texture2D? Icon => IconPath == null ? null : ResourceLoader.Load<Texture2D>(IconPath);
    protected virtual string? IconPath => $"{Id}.png".ArtistImagePath();
    public IHoverTip HoverTip => new ArtistHoverTip(ArtByName, Icon);

    public static T Get<T>() where T : Artist, new()
    {
        return (T)(Instances.TryGetValue(typeof(T), out var a) ? a : Instances[typeof(T)] = new T());
    }
}

public class KemalSag : Artist;

public class EchoPlaceholder : Artist;

public class HabibeBahce : Artist;

public class DerpyJacob : Artist;

public class AiSlop : Artist;
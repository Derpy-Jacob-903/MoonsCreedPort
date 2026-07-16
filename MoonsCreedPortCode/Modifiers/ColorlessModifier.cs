using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Models;

namespace MoonsCreedPort.MoonsCreedPortCode.Modifiers;

public abstract class ColorlessModifier : CustomModifierModel
    {
        public override int SortOrder => IsCanonical ? ModifierMap[this] : ModifierMap[ModelDb.GetById<ColorlessModifier>(Id)];

    internal static Dictionary<ColorlessModifier, int> ModifierMap
    {
        get
        {
            var field = Init();
            return field;
        }
    }

    private static Dictionary<ColorlessModifier, int> Init()
    {
        Dictionary<ColorlessModifier, string> dict = ModelDb
            .All.OfType<ColorlessModifier>()
            .ToDictionary<ColorlessModifier, ColorlessModifier, string>(prm => prm, prm => prm.Title.GetRawText().ToLowerInvariant());

        string[] values = [.. dict.Values.Order()];

        Dictionary<ColorlessModifier, int> indexDic = [];
        foreach (KeyValuePair<ColorlessModifier, string> pair in dict)
        {
            int index = values.IndexOf(dict[pair.Key]);
            indexDic.Add(pair.Key, index + 1000);
        }
        return indexDic;
    }

    protected override string IconPath => Path.Join(MoonsCreedPortMainFile.ModId, "images", "modifiers", $"{Id.Entry.ToLowerInvariant()}.png");
} 
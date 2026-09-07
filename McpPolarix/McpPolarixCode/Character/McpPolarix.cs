using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using McpPolarix.McpPolarixCode.Extensions;
using Godot;
using McpPolarix.McpPolarixCode.Cards;
using McpPolarix.McpPolarixCode.Relics;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MoonsCreedPortCard.MoonsCreedPortCardCode.Character;

namespace McpPolarix.McpPolarixCode.Character;

public class Polarix : MoonsCreedPortCharacter
{
    public const string CharacterId = "Polarix";
    //protected override CharacterModel UnlocksAfterRunAs => ModelDb.Character<Echo.Echo>();

    public static readonly Color Color = new("ae00ff");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Masculine;
    public override int StartingHp => 80;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikePolarix>(),
        ModelDb.Card<StrikePolarix>(),
        ModelDb.Card<StrikePolarix>(),
        ModelDb.Card<StrikePolarix>(),
        ModelDb.Card<RadiantStrike>(),
        ModelDb.Card<DefendPolarix>(),
        ModelDb.Card<DefendPolarix>(),
        ModelDb.Card<DefendPolarix>(),
        ModelDb.Card<DefendPolarix>(),
        ModelDb.Card<DefendPolarix>(),
        ModelDb.Card<CovetPolarix>(),
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<BurningBloodPolarix>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<PolarixCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<PolarixRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<PolarixPotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }

    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
}
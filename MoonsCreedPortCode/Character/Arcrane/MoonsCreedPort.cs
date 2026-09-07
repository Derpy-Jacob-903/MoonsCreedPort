using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Relics;
using MoonsCreedPort.MoonsCreedPortCode.Cards.Arcrane;
using MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Extensions;
using MoonsCreedPort.MoonsCreedPortCode.Relics.Arcrane;

namespace MoonsCreedPort.MoonsCreedPortCode.Character.Arcrane;

public class Arcrane : PlaceholderCharacterModel
{
    public const string CharacterId = "Arcrane";
    public override string PlaceholderID => "regent";

    public static readonly Color Color = new("bbdb44");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 75;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeArcrane>(),
        ModelDb.Card<StrikeArcrane>(),
        ModelDb.Card<StrikeArcrane>(),
        ModelDb.Card<StrikeArcrane>(),
        ModelDb.Card<DefendArcrane>(),
        ModelDb.Card<DefendArcrane>(),
        ModelDb.Card<DefendArcrane>(),
        ModelDb.Card<DefendArcrane>(),
        ModelDb.Card<ChargeCard>(),
        ModelDb.Card<MagicMissile>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<FencingManualArcrane>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<ArcraneCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<ArcraneRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<ArcranePotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override string CustomIconTexturePath => "character_icon_arcrane.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_arcrane.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_arcrane_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_arcrane.png".CharacterUiPath();
}
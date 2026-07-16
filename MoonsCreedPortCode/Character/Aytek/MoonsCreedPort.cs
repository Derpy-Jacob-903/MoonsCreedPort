using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Relics;
using MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Extensions;
using MoonsCreedPort.MoonsCreedPortCode.Relics.Aytek;

namespace MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

public class Aytek : PlaceholderCharacterModel
{
    public const string CharacterId = "Aytek";
    public override string PlaceholderID => "defect";
    public static readonly Color Color = new("f65d34");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Masculine;
    public override int StartingHp => 75;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeAytek>(),
        ModelDb.Card<StrikeAytek>(),
        ModelDb.Card<StrikeAytek>(),
        ModelDb.Card<StrikeAytek>(),
        ModelDb.Card<SimpleMissile>(),
        ModelDb.Card<SimpleMissile>(),
        ModelDb.Card<DefendAytek>(),
        ModelDb.Card<DefendAytek>(),
        ModelDb.Card<DefendAytek>(),
        ModelDb.Card<DefendAytek>(),
        ModelDb.Card<PowerBank>(),
        ModelDb.Card<TechLock>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<RingOfTheSnakeAytek>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<AytekCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<AytekRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<AytekPotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override string CustomIconTexturePath => "character_icon_aytek.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_aytek.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_aytek_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_aytek.png".CharacterUiPath();
}
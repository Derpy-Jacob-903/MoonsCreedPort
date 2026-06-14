using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Models.Relics;
using MoonsCreedPort.MoonsCreedPortCode.Cards.Echo;
using MoonsCreedPort.MoonsCreedPortCode.Extensions;
using MoonsCreedPort.MoonsCreedPortCode.Relics.Echo;

namespace MoonsCreedPort.MoonsCreedPortCode.Character.Echo;

public class Echo : PlaceholderCharacterModel
{
    public const string CharacterId = "Echo";
    public override string PlaceholderID => "silent";
    protected override CharacterModel UnlocksAfterRunAs => ModelDb.Character<Aytek.Aytek>();

    public static readonly Color Color = new("3dd9ca");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 65;
    public override int BaseOrbSlotCount => 3;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<LightBarrier>(),
        ModelDb.Card<DarknessBarrage>(),
        ModelDb.Card<Harmony>(),
        ModelDb.Card<DefendEcho>(),
        ModelDb.Card<DefendEcho>(),
        ModelDb.Card<DefendEcho>(),
        ModelDb.Card<DefendEcho>(),
        ModelDb.Card<StrikeEcho>(),
        ModelDb.Card<StrikeEcho>(),
        ModelDb.Card<StrikeEcho>(),
        ModelDb.Card<StrikeEcho>(),
        ModelDb.Card<StrikeEcho>(),
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<RingOfTheSnakeEcho>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<EchoCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<EchoRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<EchoPotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override string CustomIconTexturePath => "character_icon_echo.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_echo.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_echo_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_echo.png".CharacterUiPath();
}
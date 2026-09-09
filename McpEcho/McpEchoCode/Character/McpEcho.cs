using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using McpEcho.McpEchoCode.Extensions;
using Godot;
using McpEcho.McpEchoCode.Cards;
using McpEcho.McpEchoCode.Relics;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;
using MoonsCreedPortCard.MoonsCreedPortCardCode.Character;

namespace McpEcho.McpEchoCode.Character;

public class Echo : MoonsCreedPortCharacter
{
    public const string CharacterId = "Echo";
    public override string PlaceholderID => "silent";

    public static readonly Color Color = new("3dd9ca");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 65; // very odd
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
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }
    
    public override string CustomVisualPath => "characters/echo/echo.tscn".ImagePath();
    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
    
    protected override List<(AnimState, string)> AnimationStates
    {
        get
        {
            return new List<(AnimState, string)>()
            {
                (new AnimState("Cast"), "Cast"),
                (new AnimState("MeleeAttack2"), "Attack"),
                (new AnimState("MeleeAttack3"), "Attack_Dark"),
                (new AnimState("MeleeAttack1"), "Attack_Light"),
                (new AnimState("Hurt"), "Hit"),
                (new AnimState("Cast"), "PowerUp")
            };
        }
    }
    
    public override CreatureAnimator GenerateAnimator(MegaSprite controller, Creature creature)
    {
        AnimState state1 = new AnimState("Idle", true);
        AnimState state2 = new AnimState("Death");
        AnimState state4 = new AnimState("Idle", true);
        CreatureAnimator animator = new CreatureAnimator(state1, controller);
        animator.AddAnyState("Idle", state1);
        animator.AddAnyState("Relaxed", state4);
        animator.AddAnyState("Dead", state2);
        foreach ((AnimState, string) animationState in this.AnimationStates)
        {
            animationState.Item1.AddNextState(state1);
            animator.AddAnyState(animationState.Item2, animationState.Item1);
        }
        return animator;
    }
}
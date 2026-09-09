using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using McpPolarix.McpPolarixCode.Extensions;
using Godot;
using McpPolarix.McpPolarixCode.Cards;
using McpPolarix.McpPolarixCode.Relics;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Entities.Creatures;
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
    
    public override string CustomVisualPath => "characters/polarix/polarix.tscn".ImagePath();
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
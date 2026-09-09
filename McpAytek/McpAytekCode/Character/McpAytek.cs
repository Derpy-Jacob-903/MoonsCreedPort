using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using McpAytek.McpAytekCode.Extensions;
using Godot;
using McpAytek.McpAytekCode.Cards;
using McpAytek.McpAytekCode.Relics;
using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;

namespace McpAytek.McpAytekCode.Character;

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
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }

    public override string CustomVisualPath => "characters/aytek/aytek.tscn".ImagePath();

    public override string CustomEnergyCounterPath => "charui/energy_counter/aytek_energy_counter.tscn".ImagePath();
    public override string CustomIconTexturePath => "character_icon_aytek.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_aytek.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_aytek_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_aytek.png".CharacterUiPath();
    
    public new Func<Creature, bool> IsLowHealth => (creature => false);

    /// <summary>
    /// These are the standard animations based on the animation trigger.
    /// These animations always transition back to an idle.
    /// </summary>
    protected override List<(AnimState, string)> AnimationStates
    {
        get
        {
            return new List<(AnimState, string)>()
            {
                (new AnimState("Cast"), "Cast"),
                (new AnimState("Fist_Attack"), "Attack"),
                (new AnimState("Kick_Attack"), "Attack_Missile"),
                (new AnimState("Right_Left_fire_Attack"), "Attack_Gun_Alt"),
                (new AnimState("Right_Fire_attack"), "Attack_Gun"),
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
        animator.AddAnyState("idle_loop", state1);
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
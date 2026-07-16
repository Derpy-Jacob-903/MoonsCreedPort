using System.Runtime.Serialization;
using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Orbs;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Localization.Formatters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System;
using System.Linq;

namespace MoonsCreedPort.MoonsCreedPortCode.Powers;

public class SpentBlackOrbPower : MoonsCreedPortPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
}
public class SpentWhiteOrbPower : MoonsCreedPortPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
}

public abstract class EchoOrb<SpentPowerModel> : CustomOrbModel 
    where SpentPowerModel : PowerModel
{
    public static OrbModel GetRandomEchoOrb(Rng rng)
    {
        return ModelDb.GetById<OrbModel>(rng.NextItem(_validEchoOrbs));
    }
    private static readonly ModelId[] _validEchoOrbs =
    [
        ModelDb.GetId<WhiteEchoOrb>(),
        ModelDb.GetId<BlackEchoOrb>()
    ];
    public override async Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext context)
    {
        await PowerCmd.Apply<SpentPowerModel>(context,Owner.Creature, EvokeVal, null, null, false);
        return (IEnumerable<Creature>) Array.Empty<Creature>();
    }
    public static async Task ChannelRandomEchoOrb(PlayerChoiceContext context, Player player)
    {
        await OrbCmd.Channel(context, GetRandomEchoOrb(player.RunState.Rng.CombatOrbGeneration).ToMutable(), player);
    }
    public override decimal PassiveVal => 1;
    public override decimal EvokeVal => 1;
    public override Color DarkenedColor => new Color("004dfa");
    
    //evil and fucked up this wasn't added
    //note to self: pr this to baselib
    // ReSharper disable once MemberCanBePrivate.Global
    public static async Task EvokeSpecific(
        PlayerChoiceContext choiceContext,
        Player player,
        OrbModel evokedOrb,
        bool dequeue = true)
    {
        if (player.PlayerCombatState == null || evokedOrb == null)
            return;
        var orbQueue = player.PlayerCombatState.OrbQueue;
        if (!orbQueue.Orbs.Contains(evokedOrb))
            return;
        choiceContext.PushModel(evokedOrb);
        await Evoke2(choiceContext, player, evokedOrb, dequeue);
        choiceContext.PopModel(evokedOrb);
    }
    
    public static async Task EvokeFirstOf<OrbType>(
        PlayerChoiceContext choiceContext,
        Player player,
        bool dequeue = true)
        where OrbType : OrbModel
    {
        if (player.PlayerCombatState == null)
            return;
        var orbQueue = player.PlayerCombatState.OrbQueue;
        OrbModel orb = orbQueue.Orbs.OfType<OrbType>().FirstOrDefault();

        if (orb == null)
            return;
        choiceContext.PushModel(orb);
        await Evoke2(choiceContext, player, orb, dequeue);
        choiceContext.PopModel(orb);
    }
    
    public static async Task<int> EvokeAllOf<OrbType>(
        PlayerChoiceContext choiceContext, 
        Player player,
        Func<Task> task = null,
        bool dequeue = true)
        where OrbType : OrbModel
    {
        var count = 0;
        if (player.PlayerCombatState == null) return 0;
        var orbs = player.PlayerCombatState.OrbQueue.Orbs.ToList();
        foreach (var orb in orbs.OfType<OrbType>())
        {
            await EvokeSpecific(choiceContext, player, orb, dequeue);
            count++;
            if (task != null) await task();
        }
        return count;
    }

    private static async Task Evoke2(
        PlayerChoiceContext choiceContext,
        Player player,
        OrbModel evokedOrb,
        bool dequeue = true)
    {
        if (CombatManager.Instance.IsOverOrEnding)
            return;
        OrbQueue orbQueue = player.PlayerCombatState.OrbQueue;
        if (player.PlayerCombatState == null || player.Creature.CombatState == null || orbQueue.Orbs.Count <= 0)
            return;
        bool removed = false;
        if (dequeue)
        {
            removed = orbQueue.Remove(evokedOrb);
            NCombatRoom.Instance?.GetCreatureNode(player.Creature)?.OrbManager?.EvokeOrbAnim(evokedOrb);
        }
        choiceContext.PushModel((AbstractModel) evokedOrb);
        IEnumerable<Creature> targets = await evokedOrb.Evoke(choiceContext);
        choiceContext.PopModel((AbstractModel) evokedOrb);
        await Hook.AfterOrbEvoked(choiceContext, player.Creature.CombatState, evokedOrb, targets);
        if (!removed)
            return;
        evokedOrb.RemoveInternal();
    }
    public override Node2D? CreateCustomSprite()
    {
        var container = new Node2D();
        string lightningPath = SceneHelper.GetScenePath("orbs/orb_visuals/lightning_orb");
        Node2D lightning = PreloadManager.Cache.GetScene(lightningPath)
            .Instantiate<Node2D>(PackedScene.GenEditState.Disabled);
        new MegaSprite(lightning.GetNode("SpineSkeleton"))
            .GetAnimationState().SetAnimation("idle_loop");
        // change the color and size
        lightning.Modulate = new Color(0.8f, 0.1f, 0.0f, 1.0f);
        lightning.Scale = new Vector2(1.1f, 1.1f);
        container.AddChild(lightning);
        return container;
    }

    public static bool HasHarmonyCost(Player Owner, CardModel cardModel = null)
    {
        var orbs = Owner.PlayerCombatState.OrbQueue.Orbs;
        return Owner.PlayerCombatState != null && orbs.Any(orb => orb is BlackEchoOrb) && orbs.Any(orb => orb is WhiteEchoOrb);
    }
}

public class BlackEchoOrb : EchoOrb<SpentBlackOrbPower>
{
    protected override string ChannelSfx => "event:/sfx/characters/defect/defect_dark_channel";
    public override Color DarkenedColor => new Color("004dfa");
    public override string CustomIconPath => "res://MoonsCreedPort/images/charui/energy/black_mana_icon.png";

    public override Node2D? CreateCustomSprite()
    {
        var container = new Node2D();
        string lightningPath = SceneHelper.GetScenePath("orbs/orb_visuals/dark_orb");
        Node2D lightning = PreloadManager.Cache.GetScene(lightningPath)
            .Instantiate<Node2D>(PackedScene.GenEditState.Disabled);
        new MegaSprite(lightning.GetNode("SpineSkeleton"))
            .GetAnimationState().SetAnimation("idle_loop");
        // change the color and size
        lightning.Modulate = new Color(0.3f, 0.4f, 1.0f, 1.0f);
        container.AddChild(lightning);
        return container;
    }

}
public class WhiteEchoOrb : EchoOrb<SpentWhiteOrbPower>
{
    protected override string ChannelSfx => "event:/sfx/characters/defect/defect_glass_channel";
    public override Color DarkenedColor => new Color("004dfa");
    public override string CustomIconPath => "res://MoonsCreedPort/images/charui/energy/white_mana_icon.png";
    public override Node2D? CreateCustomSprite()
    {
        var container = new Node2D();
        string lightningPath = SceneHelper.GetScenePath("orbs/orb_visuals/glass_orb");
        Node2D lightning = PreloadManager.Cache.GetScene(lightningPath)
            .Instantiate<Node2D>(PackedScene.GenEditState.Disabled);
        new MegaSprite(lightning.GetNode("SpineSkeleton"))
            .GetAnimationState().SetAnimation("idle_loop");
        // change the color and size
        lightning.Modulate = new Color(0.5f, 1f, 0.9f, 1.0f);
        container.AddChild(lightning);
        return container;
    }
}
/*public class WhiteManaIcon : IFormatter
{
    private const string _starIconPath = "res://images/packed/sprite_fonts/star_icon.png";
    public const string starIconSprite = "[img]res://images/packed/sprite_fonts/star_icon.png[/img]";

    public string Name
    {
        get => "starIcons";
        set => throw new NotImplementedException();
    }

    public bool CanAutoDetect { get; set; }

    public bool TryEvaluateFormat(IFormattingInfo formattingInfo)
    {
        int count;
        switch (formattingInfo.CurrentValue)
        {
            case DynamicVar dynamicVar:
                count = (int) dynamicVar.PreviewValue;
                break;
            case Decimal num1:
                count = (int) num1;
                break;
            case int num2:
                count = num2;
                break;
            default:
                throw new LocException($"Unknown value='{formattingInfo.CurrentValue}' type={formattingInfo.CurrentValue?.GetType()}");
        }
        string text = string.Concat(Enumerable.Repeat<string>("[img]res://MoonsCreedPort/images/charui/energy/white_mana_icon.png[/img]", count));
        formattingInfo.Write(text);
        return true;
    }
}*/
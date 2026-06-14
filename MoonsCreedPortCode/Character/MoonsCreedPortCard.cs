using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Random;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Extensions;

namespace MoonsCreedPort.MoonsCreedPortCode.Character;

public abstract class ColorlessCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    CustomCardModel(cost, type, rarity, target)
{
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    public override string CustomPortraitPath {
        get
        {
            if (ResourceLoader.Exists($"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath())) {
                return $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
            }
            if (ResourceLoader.Exists($"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath())) {
                return $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
            }
            return RolledArt(this) ?? MissingPortraitPath;
        }
    }
    
    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190

    //Uses card_portraits/card_name.png as image path. These should be smaller images.
    public override string PortraitPath => CustomPortraitPath ?? MissingPortraitPath;
    
    public override string BetaPortraitPath {
        get
        {
            if (ResourceLoader.Exists($"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath())) {
                return $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
            }
            if (ResourceLoader.Exists($"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath())) {
                return $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
            }
            return RolledArt(this) ?? MissingPortraitPath;
        }
    }

    protected static string RolledArt(ColorlessCard c)
    {
        //return ImageHelper.GetImagePath("atlases/card_atlas.sprites/beta.tres");
        var validPool = ModelDb.AllCards
            .Where(c2 =>
                c2 is not ColorlessCard &&
                c2.Type == c.Type &&
                c.ArtRollerCase(c2))
            .ToList();
        if (validPool.Count == 0)
            return ImageHelper.GetImagePath("atlases/card_atlas.sprites/beta.tres");
        var rng = new Rng((uint)c.Id.GetHashCode());
        var card = validPool.TakeRandom(1, rng).First();
        //if (card is null) return ImageHelper.GetImagePath("atlases/card_atlas.sprites/beta.tres");
        return ImageHelper.GetImagePath($"atlases/card_atlas.sprites/{card.Pool.Title.ToLowerInvariant()}/{card.Id.Entry.ToLowerInvariant()}.tres");
    }

    protected virtual bool ArtRollerCase(CardModel card)
    {
        return card.Pool is ColorlessCardPool or StatusCardPool or CurseCardPool;
    }
    
    public bool ForceTriggerTech = false;
    public bool WillTriggerTech => CombatState != null && this is ITechKeyword && Owner.PlayerCombatState != null && Owner.PlayerCombatState.Stars >= CanonicalStarCost;
    public bool TriggeredTech(CardPlay play) => (play.Resources.StarsSpent > 0 && play.Card is ITechKeyword) || ForceTriggerTech;
    public decimal TechATK(CardPlay play) => TriggeredTech(play) ? DynamicVars["TechDamage"].BaseValue : DynamicVars.Damage.BaseValue;
    public BlockVar TechBlock(CardPlay play) => TriggeredTech(play) ? (BlockVar)DynamicVars["TechBlock"] : DynamicVars.Block;
    public override int CurrentStarCost
    {
        get
        {
            if (this is not ITechKeyword) return base.CurrentStarCost;
            return WillTriggerTech ? CanonicalStarCost : 0;
        }
    }
    
    protected override bool ShouldGlowGoldInternal => WillTriggerTech;

    public override async Task BeforeCardAutoPlayed(CardModel card, Creature target, AutoPlayType type)
    {
        if (!(card == this && WillTriggerTech))
            return;
        ForceTriggerTech = true;
        //await PlayerCmd.LoseStars(card.CanonicalStarCost, card.Owner);
    }

    public override Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Card == this && cardPlay.IsAutoPlay) ForceTriggerTech = false;
        return base.AfterCardPlayed(choiceContext, cardPlay);
    }
    
    /*public override async Task AfterCardExhausted(PlayerChoiceContext ctx, CardModel card, bool causedByEthereal)
    {
        if (card.CombatState == null || card.Keywords.Contains(AytekStuff.ArcraneAfterlife)) return;
        var a = card.CombatState.RunState.Rng.CombatTargets.NextItem(card.CombatState.HittableEnemies);
        var cardPlay = new CardPlay
        {
            Card = card,
            Target = a,
            ResultPile = PileType.Exhaust,
            Resources = default,
            IsAutoPlay = true,
            PlayIndex = 0,
            PlayCount = 0
        };
        await card.OnPlay(ctx, cardPlay);
    }*/

    Task OnTriggerTech() => Task.CompletedTask;
}
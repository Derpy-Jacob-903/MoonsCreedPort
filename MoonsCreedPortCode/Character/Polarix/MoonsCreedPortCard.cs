using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Random;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Character.Echo;
using MoonsCreedPort.MoonsCreedPortCode.Extensions;

namespace MoonsCreedPort.MoonsCreedPortCode.Character.Polarix;

[Pool(typeof(PolarixCardPool))]
public abstract class PolarixCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    CollarlessCard(cost, type, rarity, target)
{
    //Image size:
    //Normal art: 1000x760 (Using 500x380 should also work, it will simply be scaled.)
    //Full art: 606x852
    
    protected override bool ArtRollerCase(CardModel card)
    {
        //var watcther = null
        return card.Pool is IroncladCardPool or SilentCardPool or DefectCardPool;
    }

    //Smaller variants of card images for efficiency:
    //Smaller variant of fullart: 250x350
    //Smaller variant of normalart: 250x190
    
    /*public static string RolledArt(CardModel c)
    {
        var card = ModelDb.AllCards.Where(c2 => c2 is not PolarixCard or EchoCard or AytekCard or CollarlessCard && c2.Type == c.Type).TakeRandom(1, new Rng((uint)c.Id.GetHashCode())).FirstOrDefault();
        if (card is null) return ImageHelper.GetImagePath("atlases/card_atlas.sprites/beta.tres");
        return ImageHelper.GetImagePath($"atlases/card_atlas.sprites/{card.Pool.Title.ToLowerInvariant()}/{card.Id.Entry.ToLowerInvariant()}.tres");
    }*/
}
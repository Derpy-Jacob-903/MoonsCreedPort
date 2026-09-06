using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MoonsCreedPort.MoonsCreedPortCode.Character.Polarix;
using MoonsCreedPort.MoonsCreedPortCode.Extensions;

namespace MoonsCreedPort.MoonsCreedPortCode.Character.Arcrane;

[Pool(typeof(ArcraneCardPool))]
public abstract class ArcraneCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    CollarlessCard(cost, type, rarity, target)
{
    protected override bool ArtRollerCase(CardModel card)
    {
        return card.Pool is NecrobinderCardPool or RegentCardPool;
    }
    
    public override string CustomPortraitPath {
        get
        {
            if (ResourceLoader.Exists($"arcrane/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath())) {
                return $"arcrane/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
            }
            if (ResourceLoader.Exists($"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath())) {
                return $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
            }
            if (ResourceLoader.Exists($"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath())) {
                return $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
            }
            return RolledArt(this) ?? MissingPortraitPath;
        }
    }
}
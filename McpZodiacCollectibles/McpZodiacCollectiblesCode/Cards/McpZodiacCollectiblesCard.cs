using BaseLib.Abstracts;
using BaseLib.Extensions;
using Collector.CollectorCode.Cards.Token;
using McpZodiacCollectibles.McpZodiacCollectiblesCode.Extensions;
using MegaCrit.Sts2.Core.Entities.Cards;
using Collector.CollectorCode.Core;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Encounters;

namespace McpZodiacCollectibles.McpZodiacCollectiblesCode.Cards;

public abstract class McpZodiacCollectiblesCard<T>(int cost, CardType type, CardRarity rarity, TargetType target) :
    Collectible<T>(cost, type, rarity, target) where T : EncounterModel
{
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
}

public abstract class McpZodiacCollectiblesCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    Collectible<DeprecatedEncounter>(cost, type, rarity, target)
{
    public override string CustomPortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigCardImagePath();
    public override string PortraitPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
    public override string BetaPortraitPath => $"beta/{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();
}
using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Godot;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MoonsCreedPort.MoonsCreedPortCode.Character.Polarix;
using MoonsCreedPort.MoonsCreedPortCode.Extensions;

namespace MoonsCreedPort.MoonsCreedPortCode.Character.Echo;

[Pool(typeof(EchoCardPool))]
public abstract class EchoCard(int cost, CardType type, CardRarity rarity, TargetType target) :
    CollarlessCard(cost, type, rarity, target)
{
    protected override bool ArtRollerCase(CardModel card)
    {
        return card.Pool is DefectCardPool or SilentCardPool;
    }
}
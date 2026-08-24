using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Potions;
using MoonsCreedPort.MoonsCreedPortCode.Character.Echo;

namespace MoonsCreedPort.MoonsCreedPortCode.Potions;

public class ReapPotion() : EchoPotion
{
    public override string CustomPackedOutlinePath => ImageHelper.GetImagePath($"atlases/potion_outline_atlas.sprites/cunning_potion.tres");
    public override string CustomPackedImagePath => ImageHelper.GetImagePath($"atlases/potion_atlas.sprites/cunning_potion.tres");
    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyPlayer;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CardsVar(3)]
    }

    public override IEnumerable<IHoverTip> ExtraHoverTips => [ HoverTipFactory.FromCard<Shiv>(true) ]
        }
    }

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        CunningPotion cunningPotion = this;
        PotionModel.AssertValidForTargetedPotion(target);
        foreach (CardModel card in await Shiv.CreateInHand(target.Player, cunningPotion.DynamicVars.Cards.IntValue, target.CombatState, cunningPotion.Owner))
            CardCmd.Upgrade(card);
    }
}
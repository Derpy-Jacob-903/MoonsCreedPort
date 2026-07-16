using BaseLib.Abstracts;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Unlocks;
using MoonsCreedPort.MoonsCreedPortCode.Character.Arcrane;
using MoonsCreedPort.MoonsCreedPortCode.Character.Polarix;
using MoonsCreedPort.MoonsCreedPortCode.Extensions;

namespace MoonsCreedPort.MoonsCreedPortCode.Character.Echo;

public class EchoCardPool : CustomCardPoolModel
{
    public override string Title => Echo.CharacterId; //This is not a display name.
    //public override string EnergyColorName => Echo.CharacterId;

    /* These HSV values will determine the color of your card back.
    They are applied as a shader onto an already colored image,
    so it may take some experimentation to find a color you like.
    Generally they should be values between 0 and 1. */
    public override float H => 174/360f; //Hue; changes the color.
    public override float S => 0.72f; //Saturation
    public override float V => 0.85f; //Brightness

    //Alternatively, leave these values at 1 and provide a custom frame image.
    /*public override Texture2D CustomFrame(CustomCardModel card)
    {
        //This will attempt to load MoonsCreedPort/images/cards/frame.png
        return PreloadManager.Cache.GetTexture2D("cards/frame.png".ImagePath());
    }*/

    //Color of small card icons
    public override Color DeckEntryCardColor => new("3dd9ca");

    public override bool IsColorless => false;
    public override string BigEnergyIconPath => ("energy/energy_" + Title.ToLower() + ".png").CharacterUiPath();
    public override string TextEnergyIconPath => ("energy/" + Title.ToLower() + "_energy_icon.png").CharacterUiPath();
}

[HarmonyPatch(typeof(CardPoolModel))]
public static class JesterCardPoolPatch
{
    [HarmonyPatch("GetUnlockedCards")]
    [HarmonyPostfix]
    public static void Postfix(
        CardPoolModel __instance,
        UnlockState unlockState,
        CardMultiplayerConstraint multiplayerConstraint,
        ref IEnumerable<CardModel> __result)
    {
        if (__result == null)
            __result = Enumerable.Empty<CardModel>();
        
        if (__instance is ArcraneCardPool)
        {
            var ironcladCards = ModelDb.AllCharacterCardPools
                .Where(c => c is NecrobinderCardPool or RegentCardPool)
                .SelectMany(c => c.GetUnlockedCards(unlockState, multiplayerConstraint))
                .Where(c => c.BaseStarCost < 1 && isOsty(c));

            __result = __result
                .Union(ironcladCards)
                .Distinct()
                .ToList();
            return;
        }
    }

    static bool isOsty(CardModel cardModel)
    {
        return cardModel is not Bodyguard or Unleash or Afterlife or 
            Flatten or Poke or Snap or BoneShards or Calcify or 
            Fetch or HighFive or Rattle or RightHandHand or SicEm or Spur or 
            NecroMastery or Sacrifice or Squeeze or Protector or Invoke or 
            PullAggro or Cleanse or Dirge or DevourLife or Reanimate
            or Venerate or HiddenCache or BlackHole or ChildOfTheStars or 
            Radiate or Genesis;
    }
}
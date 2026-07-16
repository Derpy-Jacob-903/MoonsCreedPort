using BaseLib.Abstracts;
using Godot;
using MoonsCreedPort.MoonsCreedPortCode.Extensions;

namespace MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

public class CeylanCardPool : CustomCardPoolModel
{
    public override string Title => "Ceylan"; //This is not a display name.
    //public override string EnergyColorName => Aytek.CharacterId;

    /* These HSV values will determine the color of your card back.
    They are applied as a shader onto an already colored image,
    so it may take some experimentation to find a color you like.
    Generally they should be values between 0 and 1. */
    public override float H => 231f/360f; //Hue; changes the color.
    public override float S => 0.70f; //Saturation
    public override float V => 0.84f; //Brightness

    //Alternatively, leave these values at 1 and provide a custom frame image.
    /*public override Texture2D CustomFrame(CustomCardModel card)
    {
        //This will attempt to load MoonsCreedPort/images/cards/frame.png
        return PreloadManager.Cache.GetTexture2D("cards/frame.png".ImagePath());
    }*/
    
    public override string BigEnergyIconPath => ("energy/energy_" + Title.ToLower() + ".png").CharacterUiPath();
    public override string TextEnergyIconPath => ("energy/" + Title.ToLower() + "_energy_icon.png").CharacterUiPath();

    //Color of small card icons
    public override Color DeckEntryCardColor => new("4057d6");
    public override bool IsShared => true;
    public override bool IsColorless => false;
}
using BaseLib.Abstracts;
using McpPolarix.McpPolarixCode.Extensions;
using Godot;

namespace McpPolarix.McpPolarixCode.Character;

public class PolarixCardPool : CustomCardPoolModel
{
    public override string Title => Polarix.CharacterId; //This is not a display name.

    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();


    /* These HSV values will determine the color of your card back.
    They are applied as a shader onto an already colored image,
    so it may take some experimentation to find a color you like.
    Generally they should be values between 0 and 1. */
    public override float H => 281/360f; //Hue; changes the color.
    public override float S => 0.25f; //Saturation
    public override float V => 1.20f; //Brightness

    //Alternatively, leave these values at 1 and provide a custom frame image.
    /*public override Texture2D CustomFrame(CustomCardModel card)
    {
        //This will attempt to load McpPolarix/images/cards/frame.png
        return PreloadManager.Cache.GetTexture2D("cards/frame.png".ImagePath());
    }*/

    //Color of small card icons
    public override Color DeckEntryCardColor => new("ffffff");

    public override bool IsColorless => false;
}
using BaseLib.Abstracts;
using Godot;
using MoonsCreedPort.MoonsCreedPortCode.Extensions;

namespace MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

public class CeylanCardPool : CustomCardPoolModel
{
    public override string Title => "Ceylan"; 
    public override float H => 231f/360f; //Hue; changes the color.
    public override float S => 0.70f; //Saturation
    public override float V => 0.84f; //Brightness
    
    public override string BigEnergyIconPath => ("energy/energy_" + Title.ToLower() + ".png").CharacterUiPath();
    public override string TextEnergyIconPath => ("energy/" + Title.ToLower() + "_energy_icon.png").CharacterUiPath();

    //Color of small card icons
    public override Color DeckEntryCardColor => new("4057d6");
    public override bool IsShared => true;
    public override bool IsColorless => false;
}
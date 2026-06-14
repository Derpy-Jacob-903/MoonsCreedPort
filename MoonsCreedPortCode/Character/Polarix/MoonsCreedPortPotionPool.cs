using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;

namespace MoonsCreedPort.MoonsCreedPortCode.Character.Polarix;

public class PolarixPotionPool : CustomPotionPoolModel
{
    public override string EnergyColorName => Polarix.CharacterId;
    public override Color LabOutlineColor => Polarix.Color;
    
    protected override IEnumerable<PotionModel> GenerateAllPotions() =>
    [
        ModelDb.Potion<BloodPotion>(),
        ModelDb.Potion<Ashwater>(),
        ModelDb.Potion<SoldiersStew>(),
    ];
}
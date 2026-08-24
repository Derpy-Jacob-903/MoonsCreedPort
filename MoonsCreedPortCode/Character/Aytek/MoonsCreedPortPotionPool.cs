using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.Potions;

namespace MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

public class AytekPotionPool : CustomPotionPoolModel
{
    public override string EnergyColorName => Aytek.CharacterId;
    public override Color LabOutlineColor => Aytek.Color;

    protected override IEnumerable<PotionModel> GenerateAllPotions() =>
    [
        ModelDb.Potion<StarPotion>()
    ];
}
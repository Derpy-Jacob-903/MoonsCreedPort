using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.PotionPools;
using MegaCrit.Sts2.Core.Models.Potions;

namespace MoonsCreedPort.MoonsCreedPortCode.Character.Arcrane;

public class ArcranePotionPool : CustomPotionPoolModel
{
    public override string EnergyColorName => Arcrane.CharacterId;
    public override Color LabOutlineColor => Arcrane.Color;

    protected override IEnumerable<PotionModel> GenerateAllPotions() =>
    [
    ];
}
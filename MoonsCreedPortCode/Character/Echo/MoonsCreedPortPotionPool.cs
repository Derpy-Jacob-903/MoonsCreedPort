using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Timeline.Epochs;

namespace MoonsCreedPort.MoonsCreedPortCode.Character.Echo;

public class EchoPotionPool : CustomPotionPoolModel
{
    public override string EnergyColorName => Echo.CharacterId;
    public override Color LabOutlineColor => Echo.Color;
    protected override IEnumerable<PotionModel> GenerateAllPotions() =>
    [
        ModelDb.Potion<PoisonPotion>(),
        ModelDb.Potion<CunningPotion>(),
        ModelDb.Potion<GhostInAJar>()
    ];
}
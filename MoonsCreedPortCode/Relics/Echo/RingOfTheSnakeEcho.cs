using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Relics;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Character.Echo;
using MoonsCreedPort.MoonsCreedPortCode.Relics.Aytek;

namespace MoonsCreedPort.MoonsCreedPortCode.Relics.Echo;

public class RingOfTheSnakeEcho() : EchoRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;

    public override RelicModel GetUpgradeReplacement()
    {
        return ModelDb.Relic<RingOfTheDrake>();
    }
    
    protected override string PackedIconOutlinePath => ImageHelper.GetImagePath($"atlases/relic_outline_atlas.sprites/ring_of_the_snake.tres");
    
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(2)
    ];

    public override Decimal ModifyHandDraw(Player player, Decimal count)
    {
        return player != this.Owner || this.Owner.PlayerCombatState.TurnNumber > 1 ? count : count + this.DynamicVars.Cards.BaseValue;
    }
}
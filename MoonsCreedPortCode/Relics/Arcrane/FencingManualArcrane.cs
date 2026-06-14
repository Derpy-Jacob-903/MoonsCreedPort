using BaseLib.Utils;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.Relics;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Powers;
using MoonsCreedPort.MoonsCreedPortCode.Relics.Aytek;

namespace MoonsCreedPort.MoonsCreedPortCode.Relics.Arcrane;

public class FencingManualArcrane() : AytekRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;
    
    protected override string PackedIconOutlinePath => ImageHelper.GetImagePath($"atlases/relic_outline_atlas.sprites/fencing_manual.tres");

    public override RelicModel GetUpgradeReplacement()
    {
        return ModelDb.Relic<RingOfTheSerpent>();
    }

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new ChargeVar(12)
    ];

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner.Creature) || Owner.PlayerCombatState.TurnNumber > 1)
            return;
        Flash();
        await ChargeVar.GainCharge(new ThrowingPlayerChoiceContext(), Owner, DynamicVars[ChargeVar.defaultName].IntValue);
    }
}
using BaseLib.Utils;
using McpAytek.McpAytekCode.Relics;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.RelicPools;
using MegaCrit.Sts2.Core.Models.Relics;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

namespace McpAytek.McpAytekCode.Relics;

[Pool(typeof(FallbackRelicPool))]
public class RingOfTheSerpent() : AytekRelic
{
    public override RelicRarity Rarity =>
        RelicRarity.Starter;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new CardsVar(2),
        new StarsVar(4)
    ];

    public override Decimal ModifyHandDraw(Player player, Decimal count)
    {
        return player != this.Owner || this.Owner.PlayerCombatState.TurnNumber > 1 ? count : count + this.DynamicVars.Cards.BaseValue;
    }
    
    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains<Creature>(Owner.Creature) || Owner.PlayerCombatState.TurnNumber > 1)
            return;
        await PlayerCmd.GainStars(DynamicVars.Stars.BaseValue, Owner);
    }
}
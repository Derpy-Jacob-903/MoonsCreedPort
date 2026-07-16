using System.Reflection;
using System.Reflection.Emit;
using BaseLib.Hooks;
using BaseLib.Utils;
using BaseLib.Utils.Patching;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace MoonsCreedPort.MoonsCreedPortCode.Powers.Echo;

public class SoldSoulPower : MoonsCreedPortPower, ISoldSoulHook
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.None;
    public (OrbModel orb, bool run) TryModifyChannel(PlayerChoiceContext context, OrbModel orb, Player player)
    {
        return (orb, orb is not WhiteEchoOrb);
    }
}

public interface ISoldSoulHook
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context">The context with which to handle player choices.</param>
    /// <param name="orb">Orb to channel.</param>
    /// <param name="player">Player who is channeling the Orb.</param>
    /// <param name="willChannel">Whether to Channel the Orb. Returning false will skip channeling.</param>
    /// <returns>
    /// The Orb to channel, and whether to skip channeling it.
    /// </returns>
    public (OrbModel orb, bool run) TryModifyChannel(
        PlayerChoiceContext context,
        OrbModel orb,
        Player player,
        bool willChannel)
        => (orb, true);
}

[HarmonyPatch(typeof(OrbCmd), nameof(OrbCmd.Channel), MethodType.Async)]
public static class ModifyHealAmountPatches
{
//amount = Hook.ModifyHealAmount(creature.Player?.RunState ?? creature.CombatState?.RunState ?? NullRunState.Instance, creature.CombatState, creature, amount);
    [HarmonyPatch(typeof(OrbCmd), nameof(OrbCmd.Channel))]
    public static class ChannelPatch
    {
        static bool Prefix(
            PlayerChoiceContext context,
            OrbModel orb,
            Player player)
        {
            var combatState = BetaMainCompatibility.Creature_.CombatState.Get(player.Creature);
            var runState = player.RunState ?? (combatState == null ? NullRunState.Instance : new CombatStateWrapper(combatState).RunState);
            var num = true;
            foreach (var item in BetaMainCompatibility.RunState.IterateHookListeners.Invoke<IEnumerable<AbstractModel>>(runState, combatState) ?? [])
            {
                if (item is ISoldSoulHook mod)
                {
                    var balls = mod.TryModifyChannel(context, orb, player, num);
                    orb = balls.orb;
                    num = balls.run;
                }
            }
            return num; // Continue with the original
        }
    }
}
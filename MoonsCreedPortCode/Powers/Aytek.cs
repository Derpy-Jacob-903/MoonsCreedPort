using System.Diagnostics.CodeAnalysis;
using BaseLib.Abstracts;
using BaseLib.Patches.Content;
using HarmonyLib;
using MegaCrit.Sts2.Core.AutoSlay;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Cards.Polarix;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Powers;

// ReSharper disable once CheckNamespace
namespace MoonsCreedPort.MoonsCreedPortCode;

public class AytekStuff
{
    [CustomEnum]
    public static CardTag Reap;
    [CustomEnum]
    public static CardTag WhiteReap;
    [CustomEnum]
    public static CardTag BlackReap;
    [CustomEnum]
    public static CardTag Missile;
    [CustomEnum]
    public static CardTag Kick;
    //HERMIT(MOD)-ROUNDHOUSE_KICK, WATCHER-WHEEL_KICK, KINGLY_KICK
    [CustomEnum] [KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Crude;
    [CustomEnum] [Obsolete("Use GunKeyword")]
    public static ValueProp GunDamage;
    [CustomEnum] [KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword GunKeyword;
    [CustomEnum] [KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword ArcraneCast;
    [CustomEnum] [KeywordProperties(AutoKeywordPosition.Before)] [Obsolete("Unused")]
    public static CardKeyword ArcranePyre;
    [CustomEnum] [KeywordProperties(AutoKeywordPosition.Before)] [Obsolete("Unused")]
    public static CardKeyword ArcraneAfterlife;
    public static bool IsGun(CardModel card)
    {
        return card.Keywords.Contains(GunKeyword);
    }
    public static bool IsCrude(CardModel card)
    {
        return card.Type == CardType.Curse || card.Rarity == CardRarity.Status || card.Rarity == CardRarity.Quest;
    }
    public static Creature GetGunTargets(Player player, ICombatState combatState)
    {
        var n = player.Creature != null ? combatState.GetOpponentsOf(player.Creature) : throw new InvalidOperationException("We require an attacker to be able to grab its opponents");
        var validTargets = n.Where((Func<Creature, bool>) (c => c.IsAlive)).ToList();
        return player.RunState.Rng.CombatTargets.NextItem(validTargets);
    }
    [HarmonyPatch(typeof(CardModel))]
    public static class OnPlayPatch
    {
        /*[HarmonyPatch(nameof(CardModel.TargetType))]
        [HarmonyPrefix]
        public static bool Prefix(CardModel __instance, ref TargetType __result)
        {
            if (__instance == null || !__instance.Keywords.Contains(GunKeyword)) return true;
            __result = TargetType.RandomEnemy;
            return false;
        }*/
        [HarmonyPatch(nameof(CardModel.OnPlayWrapper))]
        [HarmonyPrefix]
        public static bool OnPlayPrefix(CardModel __instance, 
            PlayerChoiceContext choiceContext,
            ref Creature target,
            bool isAutoPlay,
            ResourceInfo resources,
            bool skipCardPileVisuals)
        {
            if (__instance == null || !__instance.Keywords.Contains(GunKeyword) || isAutoPlay) return true;
            target = GetGunTargets(__instance.Owner, __instance.CombatState);
            return true;
        }
        [HarmonyPatch(nameof(CardModel.TargetType), MethodType.Getter)]
        [HarmonyPrefix]
        public static bool Prefix(CardModel __instance, ref TargetType __result)
        {
            if (__instance == null || !__instance.Keywords.Contains(GunKeyword)) return true;
            __result = TargetType.RandomEnemy;
            return false;
        }
        [HarmonyPatch(nameof(CardModel.IsBasicStrikeOrDefend), MethodType.Getter)]
        public static class Card_IsBasicStrikeOrDefend_Patch
        {
            [HarmonyPostfix]
            static void Postfix(CardModel __instance, ref bool __result)
            {
                if (__instance is RadiantStrike)
                    __result = false;
            }
        }
    }
    [HarmonyPatch(typeof(AbstractModel))]
    public static class ModifyDamageAdditivePatch
    {
        [HarmonyPatch(nameof(AbstractModel.ModifyDamageAdditive))]
        [HarmonyPrefix]
        public static bool Prefix(AbstractModel __instance,
            Creature target,
            Decimal amount,
            ValueProp props,
            Creature dealer,
            CardModel cardSource,
            CardPlay cardPlay, ref decimal __result)
        {
            if (cardSource == null || !cardSource.Keywords.Contains(GunKeyword)) return true;
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (__instance is not StrengthPower or IGunBlacklist) return true;
            var power = (PowerModel)__instance;
            if (power.Owner != cardSource.Owner.Creature) return true;
            __result = 0m;
            return false;
        }
    }
    
    /*[HarmonyPatch(typeof(CardKeywordExtensions), nameof(CardKeywordExtensions.GetCardText))]
    public static class KeywordColorPatch
    {
        [HarmonyPostfix]
        private static void Postfix(CardKeyword keyword, ref string __result)
        {
            if (keyword is not AytekStuff.)
            __result = __result.Replace("[gold]", $"[{color}]")
                .Replace("[/gold]", $"[/{color}]");
        }
    }*/
}

/// <summary>
/// Blocks a AbstractModel from applying its ModifyDamageAdditive hook on Gun cards.
/// </summary>
public interface IGunBlacklist {}

public class TechPointVar(string name, int techPoints) : DynamicVar(name, techPoints)
{
    public const string defaultName = "TechPointVar";

    public TechPointVar(int techPoints)
        : this("TechPointVar", techPoints)
    {
    }

    public static async Task GainTP(Player owner, decimal amount)
    {
        await PlayerCmd.GainStars(amount, owner);
    }
    public static async Task GainTP(CardModel Card, decimal amount)
    {
        await GainTP(Card.Owner, amount);
    }
    public static async Task GainTP(CardModel Card)
    {
        await GainTP(Card.Owner, Card.DynamicVars["TechPointVar"].BaseValue);
    }
    
    public static IEnumerable<DynamicVar> MakeTechDamage(
        int baseVal,
        Func<CardModel, Creature?, Decimal> bonus,
        int mult = 1,
        ValueProp props = ValueProp.Move)
    {
        return CustomCardModel.FinishMakeCalculatedVar(new CalculatedDamageVar(props).WithMultiplier(bonus), baseVal, mult);
    }
}

public class ChargeVar(string name, int charge) : PowerVar<ArcraneChargePower>(name, charge)
{
    public const string defaultName = "Charge";

    public ChargeVar(int charge)
        : this(defaultName, charge)
    {
    }

    public static async Task GainCharge(PlayerChoiceContext context, Player owner, decimal amount, CardModel card = null)
    {
        await PowerCmd.Apply<ArcraneChargePower>(context, owner.Creature, amount, (Creature)null, card);
    }
    public static async Task GainCharge(PlayerChoiceContext context, CardModel Card, decimal amount)
    {
        await GainCharge(context, Card.Owner, amount, Card);
    }
    public static async Task GainCharge(PlayerChoiceContext context, CardModel Card)
    {
        await GainCharge(context, Card.Owner, Card.DynamicVars[defaultName].BaseValue, Card);
    }
}
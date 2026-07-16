using HarmonyLib;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Polarix;
// ReSharper disable ArrangeTypeMemberModifiers

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Polarix;

public class StrikePolarixCursed() : PolarixCard(1,
    CardType.Curse, CardRarity.Basic,
    TargetType.AnyEnemy)
{
    public override int MaxUpgradeLevel => 0;
    
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Strike };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[]
        {
            new DamageVar(6m, ValueProp.Move)
        };
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (play.Target != null)
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .FromCard(play.Card, play).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(context);
    }

    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
    
    public override async Task AfterShuffle(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner && Pile != null && Pile.Type != PileType.Exhaust) return;
        await CardCmd.Exhaust(choiceContext, this);
    }
}

[HarmonyPatch(typeof(CardModel), "get_BannerMaterialPath")]
public static class FramePathPatch
{
    static void Postfix(CardModel __instance, ref string __result)
    {
        if (__instance is StrikePolarixCursed or DefendPolarixCursed && __instance.Rarity != CardRarity.Ancient)
        {
            __result = "res://materials/cards/banners/card_banner_curse_mat.tres";
        }
    }
} 
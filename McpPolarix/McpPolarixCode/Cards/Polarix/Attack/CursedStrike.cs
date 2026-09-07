using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Cards.Polarix;

namespace McpPolarix.McpPolarixCode.Cards;

public class CursedStrike() : PolarixCard(0,
    CardType.Attack, CardRarity.Common, //Common
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(12m, ValueProp.Move)
    ];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [ HoverTipFactory.FromCard<Haunt>() ];

    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (play.Target != null)
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .FromCard(play.Card, play).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(context);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.AddGeneratedCardToCombat(CombatState.CreateCard<Haunt>(Owner), PileType.Draw, Owner));
        await Cmd.Wait(0.5f);
    }
    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(6M);
}
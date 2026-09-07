using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace McpPolarix.McpPolarixCode.Cards;

public class MaleficStrike() : PolarixCard(1,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(9m, ValueProp.Move)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        var john = await CovetPolarix.CovetCmd(context, Owner, 1, this);
        if (play.Target != null)
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .FromCard(play.Card, play).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(context);
        if (john > 0)
        {
            await CommonActions.Draw(this, context);
        }
        await Cmd.Wait(0.5f);
    }
    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
}
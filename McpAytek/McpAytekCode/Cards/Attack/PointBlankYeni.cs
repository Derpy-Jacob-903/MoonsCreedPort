using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Powers.Aytek;

namespace McpAytek.McpAytekCode.Cards;

public class PointBlankYeni() : AytekCard(1,
    CardType.Attack, CardRarity.Common,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        //..MakeCalculatedDamage(6, (_, c) => c != null && c.Block > 0 ? 1 : 0, 4)
        new DamageVar(6m, ValueProp.Move),
        //new DamageVar("BlockDamage", 10m, ValueProp.Move),
        //new DynamicVar("ExtraDamage", 4M)
        new PowerVar<BruisePower>(3)
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        AytekStuff.GunKeyword
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (play.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(play.Card, play).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(context);
            await CommonActions.Apply<BruisePower>(context, play.Target, this);
        }
    }
    //public decimal ATK(CardPlay play) => play.Target?.Block > 0 ? DynamicVars["BlockDamage"].BaseValue : DynamicVars.Damage.BaseValue;
    protected override void OnUpgrade() => this.DynamicVars["BruisePower"].UpgradeValueBy(2M);
}
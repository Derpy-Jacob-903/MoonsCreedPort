using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode;
using MoonsCreedPort.MoonsCreedPortCode.Powers;

namespace McpEcho.McpEchoCode.Cards;

public class StrikeEcho() : EchoCard(1,
    CardType.Attack, CardRarity.Basic,
    TargetType.AnyEnemy)
{
    
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Strike, AytekStuff.Reap };

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
        await EchoOrb<PowerModel>.ReapKeywordCmd(context, Owner);
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
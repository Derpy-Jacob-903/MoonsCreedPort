
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Echo;
using MoonsCreedPort.MoonsCreedPortCode.Powers;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Echo;

public class DarknessBarrageEx() : EchoCard(0,
    CardType.Attack, CardRarity.Ancient,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(11m, ValueProp.Move),
            new CalculationBaseVar(0M),
            new CalculationExtraVar(1M),
            new CalculatedVar("CalculatedHits").WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => card.Owner.PlayerCombatState.OrbQueue.Orbs.Count<OrbModel>((Func<OrbModel, bool>) (c => c is BlackEchoOrb)))),
            new PowerVar<WeakPower>(3M)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (play.Target != null)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .WithHitCount((int) ((CalculatedVar) this.DynamicVars["CalculatedHits"]).Calculate(play.Target))
                .FromCard(play.Card, play).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(context);
            await PowerCmd.Apply<WeakPower>(context, play.Target, DynamicVars.Weak.BaseValue, Owner.Creature, this);
            var times = await EchoOrb<PowerModel>.EvokeAllOf<BlackEchoOrb>(context, Owner);
        }
    }
    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(5M);
}
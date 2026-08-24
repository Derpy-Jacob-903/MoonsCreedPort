using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Powers.Aytek;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class SupressiveFire() : AytekCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(1m, ValueProp.Move),
        new RepeatVar(3),
        new PowerVar<BruisePower>(2)
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords =>
    [
        AytekStuff.GunKeyword
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (CombatState != null)
        {
            List<Creature> validTargets =  CombatState.GetOpponentsOf(Owner.Creature).Where((Func<Creature, bool>) (c => c.IsAlive)).ToList();
            //List<DamageResult> damageResults = [];
            for (int i = 0; i < DynamicVars.Repeat.BaseValue; i++)
            {
                var singleTarget = Owner.RunState.Rng.CombatTargets.NextItem(validTargets);
                var damageResult =
                    await CreatureCmd.Damage(context, singleTarget, DynamicVars.Damage, play.Card, play);
                await PowerCmd.Apply<BruisePower>(context, singleTarget, DynamicVars["BruisePower"].BaseValue, Owner.Creature, this);
                //damageResults.AddRange(damageResult);
            }
        }
    }
    protected override void OnUpgrade() => this.DynamicVars["BruisePower"].UpgradeValueBy(1M);
}
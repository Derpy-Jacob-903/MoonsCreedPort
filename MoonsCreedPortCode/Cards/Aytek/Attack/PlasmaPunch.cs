using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class PlasmaPunch() : AytekCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(2m, ValueProp.Move),
        new RepeatVar(3),
        new TechPointVar(1)
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
            List<DamageResult> damageResults = [];
            for (int i = 0; i < DynamicVars.Repeat.BaseValue; i++)
            {
                var singleTarget = Owner.RunState.Rng.CombatTargets.NextItem(validTargets);
                var damageResult =
                    await CreatureCmd.Damage(context, singleTarget, DynamicVars.Damage, play.Card, play);
                if (damageResults.Any(e => e.Receiver == damageResult.First().Receiver))
                    await TechPointVar.GainTP(this);
                damageResults.AddRange(damageResult);
            }
        }
    }
    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
}
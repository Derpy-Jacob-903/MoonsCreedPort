using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Cards.Echo;
using MoonsCreedPort.MoonsCreedPortCode.Character;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Character.Echo;
using MoonsCreedPort.MoonsCreedPortCode.Powers;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Aytek;

public class PerfectedStrikeAytek : AytekCard
{
    public PerfectedStrikeAytek() : base(1,
        CardType.Attack, CardRarity.Token,
        TargetType.AnyEnemy)
    {
        AncientsAwakenedCompat.RegisterPerfectedStrikeForCoco(ModelDb.Character<Character.Aytek.Aytek>(), ModelDb.Card<StrikeAytek>(), this);
    }

    public override CardPoolModel VisualCardPool => ModelDb.CardPool<PerfectedPool>();
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Strike, AytekStuff.Reap };

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        new DynamicVar[]
        {
            new DamageVar(9m, ValueProp.Move),
            new CardsVar(2)
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
        await CardPileCmd.Draw(context, DynamicVars.Cards.BaseValue, Owner);
    }

    protected override void OnUpgrade() => EnergyCost.UpgradeBy(-1);
}
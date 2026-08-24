using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Arcrane;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Powers;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Arcrane;

public class MagicMissile() : ArcraneCard(2,
    CardType.Attack, CardRarity.Basic,
    TargetType.AnyEnemy), ITranscendenceCard
{
    protected override HashSet<CardTag> CanonicalTags => [ AytekStuff.Missile ];
    public override IEnumerable<CardKeyword> CanonicalKeywords => [ CardKeyword.Retain ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        //new DamageVar(10M,ValueProp.Move),
        ..MakeCalculatedDamage(10, (Func<CardModel, Creature, Decimal>) ((card, _) => card.Owner.Creature.GetPowerAmount<ArcraneChargePower>())),
        new ChargeVar(0)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (play.Target != null)
        {
            await DamageCmd.Attack(DynamicVars.CalculatedDamage)
                .FromCard(play.Card, play).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(context);
        }
        if (IsUpgraded)
        {
            await PowerCmd.Apply<ArcraneChargePower>(context, Owner.Creature, DynamicVars["Charge"].BaseValue, null, this);
        }
    }
    protected override void OnUpgrade() => this.DynamicVars[ChargeVar.defaultName].UpgradeValueBy(5M);
    public CardModel GetTranscendenceTransformedCard()
    {
        return ModelDb.Card<MagicMissileEx>();
    }
}
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
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
    public override IEnumerable<CardKeyword> CanonicalKeywords => [ CardKeyword.Retain, AytekStuff.ArcraneCast ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new DamageVar(10M,ValueProp.Move),
        //new CalculatedDamageVar(ValueProp.Move).WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => this.WillTriggerTech ? 1 : 0))
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        if (play.Target != null)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                .FromCard(this).Targeting(play.Target)
                .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                .Execute(context);
        }
    }
    protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(5M);
    public CardModel GetTranscendenceTransformedCard()
    {
        return ModelDb.Card<MagicMissileEx>();
    }
}
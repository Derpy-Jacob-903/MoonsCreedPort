using BaseLib.Cards.Variables;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MoonsCreedPort.MoonsCreedPortCode.Character.Aytek;
using MoonsCreedPort.MoonsCreedPortCode.Character.Echo;
using MoonsCreedPort.MoonsCreedPortCode.Character.Polarix;

namespace MoonsCreedPort.MoonsCreedPortCode.Cards.Polarix;

public class ShieldSurge() : PolarixCard(1,
    CardType.Attack, CardRarity.Uncommon,
    TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars =>
    [
        new BlockVar(8m, ValueProp.Move),
        new DisplayVar<ShieldSurge>("Block2", var => (this.DynamicVars.Block.BaseValue * 2).ToString()),
        ..MakeCalculatedVar("CalculatedHits", 0, (card, creature) => Case(card) ? 0 : 1)
    ];
    protected override async Task OnPlay(
        PlayerChoiceContext context,
        CardPlay play)
    {
        await CreatureCmd.GainBlock(Owner.Creature, base.DynamicVars.Block, play);
        if (!Case(this)) return;
        if (CombatState != null)
                await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue)
                    .FromCard(play.Card, play).TargetingRandomOpponents(CombatState)
                    .WithHitFx("vfx/vfx_attack_slash", null, "blunt_attack.mp3")
                    .Execute(context);
    }

    private static bool Case(CardModel card)
    {
        return card.Owner.Creature.Block > card.DynamicVars["Block2"].BaseValue;
    }

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(3M);
}
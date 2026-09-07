using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace MoonsCreedPort.MoonsCreedPortCode.Powers.Aytek;

[Obsolete("Plasma Punch handles its {singleStarIcon} gain itself.")]
public class IonizedPower : MoonsCreedPortPower
{
    public override PowerType Type => PowerType.None;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Remove<IonizedPower>(this.Owner);
    }

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props,
        Creature dealer, CardModel cardSource)
    {
        if (cardSource is PlasmaPunch)
        {
            await PlayerCmd.GainStars(amount: this.Amount, cardSource.Owner);
            await PowerCmd.Remove<IonizedPower>(this.Owner);
        }
    }
}